using Microsoft.EntityFrameworkCore;
using WebApplication1.CourseDbContext;
using WebApplication1.DTO;
using WebApplication1.Models;

namespace WebApplication1.Service
{
    public class RecipeService
    {
        private readonly ApplicationDbContext _context;

        public RecipeService(ApplicationDbContext context)
        {
            _context = context;
        }

        //  Récupérer toutes les recettes
        public async Task<IEnumerable<Recipe>> GetAllRecipes()
        {
            return await _context.Recipes
                .Include(r => r.Ingredients)
                .Include(r => r.Steps)
                .Include(r => r.Comments)
                .ToListAsync();
        }

        //  Récupérer une recette par ID
        public async Task<Recipe?> GetRecipeById(int id)
        {
            return await _context.Recipes
                .Include(r => r.Ingredients)
                .Include(r => r.Steps)
                .Include(r => r.Comments)
                .FirstOrDefaultAsync(r => r.RecipeId == id);
        }

        //  Ajouter une nouvelle recette
        public async Task<Recipe> CreateRecipe(RecipeDTO recipeDto, string userId)
        {
            var recipe = new Recipe
            {
                Title = recipeDto.Title,
                Description = recipeDto.Description,
                PreparationTime = recipeDto.PreparationTime,
                CookingTime = recipeDto.CookingTime,
                Difficulty = recipeDto.Difficulty,
                Budget = recipeDto.Budget,
                DietType = recipeDto.DietType,
                CreatedBy = userId,
                Ingredients = recipeDto.Ingredients.Select(i => new Ingredient
                {
                    Name = i.Name,
                    Quantity = i.Quantity,
                    Unit = i.Unit
                }).ToList(),
                Steps = recipeDto.Steps.Select(s => new Step
                {
                    Description = s.Description,
                    Order = s.Order
                }).ToList()
            };

            _context.Recipes.Add(recipe);
            await _context.SaveChangesAsync();
            return recipe;
        }

        //  Modifier une recette (seulement par son créateur)
        public async Task<bool> UpdateRecipe(int id, RecipeDTO recipeDto, string userId)
        {
            var recipe = await _context.Recipes.FindAsync(id);
            if (recipe == null || recipe.CreatedBy != userId)
                return false;

            recipe.Title = recipeDto.Title;
            recipe.Description = recipeDto.Description;
            recipe.PreparationTime = recipeDto.PreparationTime;
            recipe.CookingTime = recipeDto.CookingTime;
            recipe.Difficulty = recipeDto.Difficulty;
            recipe.Budget = recipeDto.Budget;
            recipe.DietType = recipeDto.DietType;

            _context.Recipes.Update(recipe);
            await _context.SaveChangesAsync();
            return true;
        }

        //  Supprimer une recette (seulement par son créateur)
        public async Task<bool> DeleteRecipe(int id, string userId)
        {
            var recipe = await _context.Recipes.FindAsync(id);
            if (recipe == null || recipe.CreatedBy != userId)
                return false;

            _context.Recipes.Remove(recipe);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}

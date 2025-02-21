using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using WebApplication1.CourseDbContext;
using WebApplication1.DTO;
using WebApplication1.Models;
using WebApplication1.Repositories;

namespace WebApplication1.Service
{
    public class RecipeService
    {
        public RecipeRepository _recipeRepository { get; set; }
        public IngredientRepository _ingredientRepository { get; set; }

        public StepRepository _stepRepository { get; set; }

        public RecipeService(RecipeRepository recipeRepository, IngredientRepository ingredientRepository, StepRepository stepRepository)
        {
            _recipeRepository = recipeRepository;
            _ingredientRepository = ingredientRepository;
            _stepRepository = stepRepository;
        }


        //  Récupérer toutes les recettes
        public async Task<IEnumerable<Recipe>> GetAllRecipes()
        {
            return await _recipeRepository.GetAllRecipes();
        }

        //  Récupérer une recette par ID
        public async Task<RecipeDTO?> GetRecipeById(int id)
        {
            Recipe recipe = await _recipeRepository.GetRecipeById(id);

            if(recipe == null)
            {
                throw new Exception("Impossible de trouver la recette.");
            }

            List<IngredientDTO> listeIngredients = new List<IngredientDTO>();
            IEnumerable<Ingredient> ingredients = await _ingredientRepository.GetIngredientByRecipeId(id);
            foreach (var ingredient in ingredients)
            {
                var iToAdd = new IngredientDTO()
                {
                    IngredientId = ingredient.IngredientId,
                };
                listeIngredients.Add(iToAdd);
            }

            List<StepDTO> listeSteps = new List<StepDTO>();
            IEnumerable<Step> steps = await _stepRepository.GetStepByRecipeId(id);
            foreach (var step in steps)
            {
                var sToAdd = new StepDTO()
                {
                    StepId = step.StepId,
                };
                listeSteps.Add(sToAdd);
            }

            return new RecipeDTO()
            {
                Title = recipe.Title,
                Description = recipe.Description,
                PreparationTime = recipe.PreparationTime,
                CookingTime = recipe.CookingTime,
                Difficulty = recipe.Difficulty,
                Budget = recipe.Budget,
                DietType = recipe.DietType,
                UserId = recipe.CreatedBy,
                Ingredients = listeIngredients,
                Steps = listeSteps

            };
        }

        //  Ajouter une nouvelle recette
        public async Task<Recipe> CreateRecipe(RecipeDTO recipeDto)
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
                CreatedBy = recipeDto.UserId,
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
            recipe = await _recipeRepository.CreateRecipe(recipe);
            return recipe;
        }

        //  Modifier une recette (seulement par son créateur)
        public async Task<bool> UpdateRecipe(int id, RecipeUpdateDTO recipeDto)
        {
            var recipe = await _recipeRepository.GetRecipeById(id);
            if (recipe == null || recipe.CreatedBy != recipeDto.UserId)
                return false;

            recipe.Title = recipeDto.Title;
            recipe.Description = recipeDto.Description;
            recipe.PreparationTime = recipeDto.PreparationTime;
            recipe.CookingTime = recipeDto.CookingTime;
            recipe.Difficulty = recipeDto.Difficulty;
            recipe.Budget = recipeDto.Budget;
            recipe.DietType = recipeDto.DietType;

            await _recipeRepository.UpdateRecipe(recipe);
            return true;
        }

        //  Supprimer une recette (seulement par son créateur)
        public async Task<bool> DeleteRecipe(int id, int UserId)
        {
            var recipe = await _recipeRepository.GetRecipeById(id);
            if (recipe == null || recipe.CreatedBy != UserId)
                return false;


            await _recipeRepository.DeleteRecipe(recipe);
            return true;
        }
    }
}

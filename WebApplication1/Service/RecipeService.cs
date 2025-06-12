using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.DTO;
using WebApplication1.Models;
using WebApplication1.Repositories;

namespace WebApplication1.Service
{
    public class RecipeService
    {
        private readonly RecipeRepository _repo;

        public RecipeService(RecipeRepository repo)
        {
            _repo = repo;
        }

        // Récupère toutes les recettes
        public async Task<List<RecipeDTO>> GetAllRecipes()
        {
            var toutes = await _repo.GetAllAsync();
            return toutes.Select(r => new RecipeDTO
            {
                RecipeId = r.RecipeId,
                Title = r.Title,
                Description = r.Description,
                PreparationTime = r.PreparationTime,
                CookingTime = r.CookingTime,
                Servings = r.Servings,    // ajouté
                Category = r.Category,    // ajouté
                ImageUrl = r.ImageUrl,    // ajouté
                Difficulty = r.Difficulty,
                Budget = r.Budget,
                DietType = r.DietType,
                UserId = r.CreatedBy,
                Ingredients = r.Ingredients
                                   .Select(i => new IngredientDTO { Name = i.Name })
                                   .ToList(),
                Steps = r.Steps
                                   .Select(s => new StepDTO { Description = s.Description, Order = s.Order })
                                   .ToList()
            }).ToList();
        }

        // Récupère une recette par son ID
        public async Task<RecipeDTO> GetRecipeById(int id)
        {
            var r = await _repo.GetByIdAsync(id);
            if (r == null) return null;

            return new RecipeDTO
            {
                RecipeId = r.RecipeId,
                Title = r.Title,
                Description = r.Description,
                PreparationTime = r.PreparationTime,
                CookingTime = r.CookingTime,
                Servings = r.Servings,    // ajouté
                Category = r.Category,    // ajouté
                ImageUrl = r.ImageUrl,    // ajouté
                Difficulty = r.Difficulty,
                Budget = r.Budget,
                DietType = r.DietType,
                UserId = r.CreatedBy,
                Ingredients = r.Ingredients
                                   .Select(i => new IngredientDTO { Name = i.Name })
                                   .ToList(),
                Steps = r.Steps
                                   .Select(s => new StepDTO { Description = s.Description, Order = s.Order })
                                   .ToList()
            };
        }

        // Crée une nouvelle recette
        public async Task<RecipeDTO> CreateRecipe(RecipeDTO dto)
        {
            var entite = new Recipe
            {
                Title = dto.Title,
                Description = dto.Description,
                PreparationTime = dto.PreparationTime,
                CookingTime = dto.CookingTime,
                Servings = dto.Servings,     // ajouté
                Category = dto.Category,     // ajouté
                ImageUrl = dto.ImageUrl,     // ajouté
                Difficulty = dto.Difficulty,
                Budget = dto.Budget,
                DietType = dto.DietType,
                CreatedBy = dto.UserId,
                Ingredients = dto.Ingredients
                                   .Select(i => new Ingredient { Name = i.Name })
                                   .ToList(),
                Steps = dto.Steps
                                   .Select(s => new Step { Description = s.Description, Order = s.Order })
                                   .ToList()
            };

            var cree = await _repo.AddAsync(entite);

            // On renvoie le DTO complet, avec l'ID généré
            return new RecipeDTO
            {
                RecipeId = cree.RecipeId,
                Title = cree.Title,
                Description = cree.Description,
                PreparationTime = cree.PreparationTime,
                CookingTime = cree.CookingTime,
                Servings = cree.Servings,    // ajouté
                Category = cree.Category,    // ajouté
                ImageUrl = cree.ImageUrl,    // ajouté
                Difficulty = cree.Difficulty,
                Budget = cree.Budget,
                DietType = cree.DietType,
                UserId = cree.CreatedBy,
                Ingredients = cree.Ingredients
                                   .Select(i => new IngredientDTO { Name = i.Name })
                                   .ToList(),
                Steps = cree.Steps
                                   .Select(s => new StepDTO { Description = s.Description, Order = s.Order })
                                   .ToList()
            };
        }

        // Met à jour une recette existante
        public async Task<bool> UpdateRecipe(int id, RecipeUpdateDTO dto)
        {
            var r = await _repo.GetByIdAsync(id);
            if (r == null) return false;

            r.Title = dto.Title;
            r.Description = dto.Description;
            r.PreparationTime = dto.PreparationTime;
            r.CookingTime = dto.CookingTime;
            r.Servings = dto.Servings;     // ajouté
            r.Category = dto.Category;     // ajouté
            r.ImageUrl = dto.ImageUrl;     // ajouté
            r.Difficulty = dto.Difficulty;
            r.Budget = dto.Budget;
            r.DietType = dto.DietType;
            r.CreatedBy = dto.UserId;

            // Ici vous pouvez mettre à jour Ingredients et Steps si nécessaire

            await _repo.UpdateAsync(r);
            return true;
        }

        // Supprime une recette
        public async Task<bool> DeleteRecipe(int id, string userId)
        {
            return await _repo.DeleteAsync(id, userId);
        }
    }
}

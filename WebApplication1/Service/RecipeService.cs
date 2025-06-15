// Service/RecipeService.cs
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

        /// <summary>
        /// Récupère toutes les recettes
        /// </summary>
        public async Task<List<RecipeDTO>> GetAllRecipes()
        {
            var all = await _repo.GetAllAsync();
            return all.Select(r => MapToDto(r)).ToList();
        }

        /// <summary>
        /// Récupère les recettes créées par un utilisateur donné
        /// </summary>
        public async Task<List<RecipeDTO>> GetRecipesByUserAsync(string userId)
        {
            var list = await _repo.GetByUserAsync(userId);
            return list.Select(r => MapToDto(r)).ToList();
        }

        /// <summary>
        /// Récupère une recette par son ID
        /// </summary>
        public async Task<RecipeDTO> GetRecipeById(int id)
        {
            var r = await _repo.GetByIdAsync(id);
            if (r == null) return null;

            return MapToDto(r);
        }

        /// <summary>
        /// Crée une nouvelle recette
        /// </summary>
        public async Task<RecipeDTO> CreateRecipe(Recipe recipe)
        {
            var created = await _repo.AddAsync(recipe);
            return MapToDto(created);
        }

        /// <summary>
        /// Met à jour une recette existante
        /// </summary>
        public async Task<bool> UpdateRecipe(int id, RecipeUpdateDTO dto)
        {
            var r = await _repo.GetByIdAsync(id);
            if (r == null) return false;

            r.Title = dto.Title;
            r.Description = dto.Description;
            r.PreparationTime = dto.PreparationTime;
            r.CookingTime = dto.CookingTime;
            r.Servings = dto.Servings;
            r.Category = dto.Category;
            r.ImageUrl = dto.ImageUrl;
            r.CreatedBy = dto.UserId;
            // TODO : gérer Ingredients/Steps si besoin

            await _repo.UpdateAsync(r);
            return true;
        }

        /// <summary>
        /// Supprime une recette si l’utilisateur est bien le créateur
        /// </summary>
        public async Task<bool> DeleteRecipe(int id, string userId)
        {
            return await _repo.DeleteAsync(id, userId);
        }

        // Méthode privée pour centraliser le mapping Entity → DTO
        private RecipeDTO MapToDto(Recipe r)
        {
            return new RecipeDTO
            {
                RecipeId = r.RecipeId,
                Title = r.Title,
                Description = r.Description,
                PreparationTime = r.PreparationTime,
                CookingTime = r.CookingTime,
                Servings = r.Servings,
                Category = r.Category,
                ImageUrl = r.ImageUrl,
                UserId = r.CreatedBy,
                Ingredients = r.Ingredients
                                      .Select(i => new IngredientDTO
                                      {
                                          Name = i.Name,
                                          Unit = i.Unit
                                      })
                                      .ToList(),
                Steps = r.Steps
                                      .Select(s => new StepDTO
                                      {
                                          Description = s.Description,
                                          Order = s.Order
                                      })
                                      .ToList()
            };
        }
    }
}

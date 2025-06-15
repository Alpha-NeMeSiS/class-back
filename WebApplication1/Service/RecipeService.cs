using System;
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
            => _repo = repo;

        public async Task<List<RecipeDTO>> GetAllRecipesAsync()
            => (await _repo.GetAllAsync())
               .Select(MapToDto)
               .ToList();

        public async Task<RecipeDTO?> GetRecipeByIdAsync(int id)
        {
            var r = await _repo.GetByIdAsync(id);
            return r == null ? null : MapToDto(r);
        }

        public async Task<List<RecipeDTO>> GetRecipesByUserAsync(string userId)
            => (await _repo.GetByUserAsync(userId))
               .Select(MapToDto)
               .ToList();

        public async Task<RecipeDTO> AddRecipeAsync(RecipeFormDto dto, string userId)
        {
            var entity = new Recipe
            {
                Title = dto.Title,
                Description = dto.Description,
                CreatedBy = userId,
                // mappez ici Ingredients, Steps, etc.
            };
            var saved = await _repo.AddAsync(entity);
            return MapToDto(saved);
        }

        public async Task<bool> UpdateRecipeAsync(int id, RecipeUpdateDTO dto, string userId)
        {
            var existing = await _repo.GetByIdAsync(id);
            if (existing == null || existing.CreatedBy != userId)
                return false;

            existing.Title = dto.Title;
            existing.Description = dto.Description;
            // mappez ici Ingredients, Steps, etc.

            await _repo.UpdateAsync(existing);
            return true;
        }

        public async Task<bool> DeleteRecipeAsync(int id, string userId)
            => await _repo.DeleteAsync(id, userId);

        /// <summary>
        /// Recherche des recettes par mot-clé
        /// </summary>
        public async Task<List<RecipeDTO>> SearchRecipesAsync(string query)
            => (await _repo.SearchAsync(query))
               .Select(MapToDto)
               .ToList();

        private RecipeDTO MapToDto(Recipe r)
            => new RecipeDTO
            {
                RecipeId = r.RecipeId,
                Title = r.Title,
                Description = r.Description,
                // mappez ici Ingredients, Steps, Comments…
            };
    }
}

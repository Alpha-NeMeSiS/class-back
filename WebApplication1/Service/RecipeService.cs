using System;
using System.IO;             
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
            // 1) Construction de l'entité
            var entity = new Recipe
                {
                 Title = dto.Title,
                 Description = dto.Description,
                 PreparationTime = dto.PreparationTime,
                 CookingTime = dto.CookingTime,
                 Servings = dto.Servings,
                 Category = dto.Category,   
                 CreatedBy = userId         
                 };

            // 2) Upload de l'image (facultatif)
            if (dto.Image != null && dto.Image.Length > 0)
                     {
                var uploadsDir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
                Directory.CreateDirectory(uploadsDir);
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(dto.Image.FileName);
                var filePath = Path.Combine(uploadsDir, fileName);
                         using var stream = File.Create(filePath);
                await dto.Image.CopyToAsync(stream);
                entity.ImageUrl = $"/uploads/{fileName}";
                   }

            // 3) Mapping des ingrédients
            entity.Ingredients = dto.Ingredients
                .Select((name, i) => new Ingredient
                {
                    Name = name,
                    Unit = dto.IngredientUnits[i],
                    Recipe = entity
                })
                .ToList();

            // 4) Mapping des étapes
            entity.Steps = dto.Instructions
                .Select((text, order) => new Step
                {
                    Order = order,
                    Description = text,
                    Recipe = entity
                })
                .ToList();

            // 5) Persistance via le repository
            var savedEntity = await _repo.AddAsync(entity);

            // 6) Conversion de l'entité en DTO et retour
            var resultDto = MapToDto(savedEntity);
            return resultDto;
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
                ImageUrl = r.ImageUrl,
                PreparationTime = r.PreparationTime,
                CookingTime = r.CookingTime,
                Servings = r.Servings,
                Category = r.Category,
                UserId = r.CreatedBy,

                Ingredients = r.Ingredients?.Select(i => new IngredientDTO
                {
                    Name = i.Name,
                    Quantity = i.Quantity,
                    Unit = i.Unit
                }).ToList(),

                Steps = r.Steps?.OrderBy(s => s.Order).Select(s => new StepDTO
                {
                    Order = s.Order,
                    Description = s.Description
                }).ToList()
            };
    }
}

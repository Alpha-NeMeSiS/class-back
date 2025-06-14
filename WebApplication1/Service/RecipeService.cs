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

        public async Task<List<RecipeDTO>> GetAllRecipes()
        {
            var all = await _repo.GetAllAsync();
            return all.Select(r => new RecipeDTO
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
            }).ToList();
        }

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

        // On prend maintenant directement l'entité Recipe (avec Unit déjà rempli)
        public async Task<RecipeDTO> CreateRecipe(Recipe recipe)
        {
            var created = await _repo.AddAsync(recipe);

            // on renvoie le DTO complet avec Unit
            return new RecipeDTO
            {
                RecipeId = created.RecipeId,
                Title = created.Title,
                Description = created.Description,
                PreparationTime = created.PreparationTime,
                CookingTime = created.CookingTime,
                Servings = created.Servings,
                Category = created.Category,
                ImageUrl = created.ImageUrl,
                UserId = created.CreatedBy,
                Ingredients = created.Ingredients
                                    .Select(i => new IngredientDTO
                                    {
                                        Name = i.Name,
                                        Unit = i.Unit
                                    })
                                    .ToList(),
                Steps = created.Steps
                                    .Select(s => new StepDTO
                                    {
                                        Description = s.Description,
                                        Order = s.Order
                                    })
                                    .ToList()
            };
        }

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
            // si tu veux gérer Ingredients/Steps en update, fais-le ici

            await _repo.UpdateAsync(r);
            return true;
        }

        public async Task<bool> DeleteRecipe(int id, string userId)
        {
            return await _repo.DeleteAsync(id, userId);
        }
    }
}

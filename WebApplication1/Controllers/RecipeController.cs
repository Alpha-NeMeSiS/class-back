// Controllers/RecipeController.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using WebApplication1.DTO;       // RecipeFormDto, IngredientDTO, StepDTO
using WebApplication1.Models;    // Recipe, Ingredient, Step
using WebApplication1.Service;   // RecipeService

namespace WebApplication1.Controllers
{
    [Route("api/recipes")]
    [ApiController]
    public class RecipeController : ControllerBase
    {
        private readonly RecipeService _recipeService;

        public RecipeController(RecipeService recipeService)
        {
            _recipeService = recipeService;
        }

        // 1️⃣ Méthode POST pour recevoir un FormData (multipart/form-data)
        [HttpPost]
        public async Task<IActionResult> CreateRecipe([FromForm] RecipeFormDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // --- 1) Upload de l’image ---
            string imageUrl = null;
            if (dto.Image != null && dto.Image.Length > 0)
            {
                var uploadsDir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
                Directory.CreateDirectory(uploadsDir);

                var uniqueFileName = Guid.NewGuid() + Path.GetExtension(dto.Image.FileName);
                var filePath = Path.Combine(uploadsDir, uniqueFileName);

                using (var stream = System.IO.File.Create(filePath))
                {
                    await dto.Image.CopyToAsync(stream);
                }

                imageUrl = $"/uploads/{uniqueFileName}";
            }

            // --- 2) Mapping DTO → Entité métier ---
            var recipe = new Recipe
            {
                Title = dto.Title,
                Description = dto.Description,
                PreparationTime = dto.PreparationTime,
                CookingTime = dto.CookingTime,
                Servings = dto.Servings,
                Category = dto.Category,
                ImageUrl = imageUrl,
                CreatedBy = dto.UserId,
                Ingredients = dto.Ingredients?
                    .Select(name => new Ingredient { Name = name })
                    .ToList()
                    ?? new List<Ingredient>(),
                Steps = dto.Instructions?
                    .Select((text, idx) => new Step { Description = text, Order = idx + 1 })
                    .ToList()
                    ?? new List<Step>()
            };

            // --- 3) Appel au service (qui renvoie un RecipeDTO) ---
            var createdDto = await _recipeService.CreateRecipe(new RecipeDTO
            {
                Title = recipe.Title,
                Description = recipe.Description,
                PreparationTime = recipe.PreparationTime,
                CookingTime = recipe.CookingTime,
                Servings = recipe.Servings,
                Category = recipe.Category,
                ImageUrl = recipe.ImageUrl,
                UserId = recipe.CreatedBy,
                Ingredients = recipe.Ingredients
                                      .Select(i => new IngredientDTO { Name = i.Name })
                                      .ToList(),
                Steps = recipe.Steps
                                      .Select(s => new StepDTO { Description = s.Description, Order = s.Order })
                                      .ToList()
            });

            // --- 4) Retourne 201 Created avec l’objet créé ---
            return CreatedAtAction(
                nameof(GetRecipeById),
                new { id = createdDto.RecipeId },
                createdDto
            );
        }

        // GET /api/recipes
        [HttpGet]
        public async Task<IActionResult> GetRecipes()
        {
            var recipes = await _recipeService.GetAllRecipes();
            return Ok(recipes);
        }

        // GET /api/recipes/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetRecipeById(int id)
        {
            var recipe = await _recipeService.GetRecipeById(id);
            if (recipe == null)
                return NotFound(new { Message = "Recette introuvable" });

            return Ok(recipe);
        }

        // PUT /api/recipes/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRecipe(int id, [FromBody] RecipeUpdateDTO recipeDto)
        {
            var result = await _recipeService.UpdateRecipe(id, recipeDto);
            if (!result)
                return NotFound(new { Message = "Recette non trouvée ou accès refusé" });

            return NoContent();
        }

        // DELETE /api/recipes/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRecipe(int id, string userId)
        {
            var result = await _recipeService.DeleteRecipe(id, userId);
            if (!result)
                return NotFound(new { Message = "Recette non trouvée ou accès refusé" });

            return NoContent();
        }
    }
}

// Controllers/RecipeController.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Security.Claims;
using WebApplication1.DTO;     // RecipeFormDto, RecipeDTO, RecipeUpdateDTO, etc.
using WebApplication1.Models;  // Recipe, Ingredient, Step
using WebApplication1.Service; // RecipeService

namespace WebApplication1.Controllers
{
    [Route("api/recipes")]
    [ApiController]
    [Authorize]   // protège tous les endpoints de ce controller
    public class RecipeController : ControllerBase
    {
        private readonly RecipeService _recipeService;

        public RecipeController(RecipeService recipeService)
        {
            _recipeService = recipeService;
        }

        // --------------------------------------------------
        // GET /api/recipes/me
        // Renvoie les recettes créées par l’utilisateur courant
        // --------------------------------------------------
        [HttpGet("me")]
        public async Task<IActionResult> GetMyRecipes()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var list = await _recipeService.GetRecipesByUserAsync(userId);
            return Ok(list);
        }

        // --------------------------------------------------
        // POST /api/recipes
        // Crée une nouvelle recette (multipart/form-data)
        // --------------------------------------------------
        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> CreateRecipe([FromForm] RecipeFormDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // 1) Upload de l'image
            string imageUrl = null;
            if (dto.Image != null && dto.Image.Length > 0)
            {
                var uploadsDir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
                Directory.CreateDirectory(uploadsDir);

                var fileName = Guid.NewGuid() + Path.GetExtension(dto.Image.FileName);
                var filePath = Path.Combine(uploadsDir, fileName);
                using (var stream = System.IO.File.Create(filePath))
                    await dto.Image.CopyToAsync(stream);
                imageUrl = $"/uploads/{fileName}";
            }

            // 2) Construction de l'entité Recipe
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
                                    .Select((name, idx) => new Ingredient
                                    {
                                        Name = name,
                                        Unit = (dto.IngredientUnits != null && dto.IngredientUnits.Length > idx)
                                               ? dto.IngredientUnits[idx]
                                               : String.Empty
                                    })
                                    .ToList()
                                  ?? new List<Ingredient>(),
                Steps = dto.Instructions?
                                    .Select((text, idx) => new Step
                                    {
                                        Description = text,
                                        Order = idx + 1
                                    })
                                    .ToList()
                                  ?? new List<Step>()
            };

            // 3) Persistance via le service
            var created = await _recipeService.CreateRecipe(recipe);

            // 4) Mapping du DTO de retour
            var resultDto = new RecipeDTO
            {
                RecipeId = created.RecipeId,
                Title = created.Title,
                Description = created.Description,
                PreparationTime = created.PreparationTime,
                CookingTime = created.CookingTime,
                Servings = created.Servings,
                Category = created.Category,
                ImageUrl = created.ImageUrl,
                UserId = created.UserId,
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

            return CreatedAtAction(
                nameof(GetRecipeById),
                new { id = resultDto.RecipeId },
                resultDto
            );
        }

        // --------------------------------------------------
        // GET /api/recipes
        // Renvoie toutes les recettes
        // --------------------------------------------------
        [HttpGet]
        public async Task<IActionResult> GetAllRecipes()
        {
            var list = await _recipeService.GetAllRecipes();
            return Ok(list);
        }

        // --------------------------------------------------
        // GET /api/recipes/{id}
        // Renvoie une recette par son ID
        // --------------------------------------------------
        [HttpGet("{id}")]
        public async Task<IActionResult> GetRecipeById(int id)
        {
            var r = await _recipeService.GetRecipeById(id);
            if (r == null) return NotFound();
            return Ok(r);
        }

        // --------------------------------------------------
        // PUT /api/recipes/{id}
        // Met à jour une recette
        // --------------------------------------------------
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRecipe(int id, [FromBody] RecipeUpdateDTO dto)
        {
            var ok = await _recipeService.UpdateRecipe(id, dto);
            if (!ok) return NotFound();
            return NoContent();
        }

        // --------------------------------------------------
        // DELETE /api/recipes/{id}?userId=...
        // Supprime une recette si l’utilisateur est le créateur
        // --------------------------------------------------
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRecipe(int id, [FromQuery] string userId)
        {
            var ok = await _recipeService.DeleteRecipe(id, userId);
            if (!ok) return NotFound();
            return NoContent();
        }
    }
}

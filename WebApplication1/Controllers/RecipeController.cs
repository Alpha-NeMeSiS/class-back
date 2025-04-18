using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.DTO;
using WebApplication1.Models;
using WebApplication1.Repositories;
using WebApplication1.Service;

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

        // Récupérer toutes les recettes
        [HttpGet]
        public async Task<IActionResult> GetRecipes()
        {
            var recipes = await _recipeService.GetAllRecipes();
            return Ok(recipes);
        }

        // Récupérer une recette par ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetRecipeById(int id)
        {
            var recipe = await _recipeService.GetRecipeById(id);
            if (recipe == null) return NotFound(new { Message = "Recette introuvable" });

            return Ok(recipe);
        }

        // Ajouter une nouvelle recette (authentification requise)
        [HttpPost]
        //[Authorize]
        public async Task<IActionResult> CreateRecipe([FromBody] RecipeDTO recipeDto)
        {

            var recipe = await _recipeService.CreateRecipe(recipeDto);
            return CreatedAtAction(nameof(GetRecipeById), new { id = recipe.RecipeId }, recipe);
        }

        // Modifier une recette (seulement par son créateur)
        [HttpPut("{id}")]
        //[Authorize]
        public async Task<IActionResult> UpdateRecipe(int id, [FromBody] RecipeUpdateDTO recipeDto)
        {


            var result = await _recipeService.UpdateRecipe(id, recipeDto);
            if (!result) return NotFound(new { Message = "Recette non trouvée ou accès refusé" });

            return NoContent();
        }

        // Supprimer une recette (seulement par son créateur)
        [HttpDelete("{id}")]
        //[Authorize]
        public async Task<IActionResult> DeleteRecipe(int id,string UserId)
        {

            var result = await _recipeService.DeleteRecipe(id,UserId);
            if (!result) return NotFound(new { Message = "Recette non trouvée ou accès refusé" });

            return NoContent();
        }
    }
}

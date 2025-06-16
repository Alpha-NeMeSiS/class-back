using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using WebApplication1.DTO;
using WebApplication1.Service;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("api/recipes")]
    public class RecipeController : ControllerBase
    {
        private readonly RecipeService _service;
        public RecipeController(RecipeService service)
            => _service = service;

        // GET /api/recipes
        [HttpGet]
        public async Task<ActionResult<List<RecipeDTO>>> GetAll()
            => Ok(await _service.GetAllRecipesAsync());

        // GET /api/recipes/search?q=...
        [HttpGet("search")]
        [AllowAnonymous]  // retirez si vous voulez authentifier
        public async Task<ActionResult<List<RecipeDTO>>> Search([FromQuery] string q)
        {
            if (string.IsNullOrWhiteSpace(q))
                return BadRequest(new { error = "Le paramètre 'q' est requis." });

            var results = await _service.SearchRecipesAsync(q);
            if (!results.Any())
                return NotFound(new { message = $"Aucune recette trouvée pour « {q} »." });

            return Ok(results);
        }

        // POST /api/recipes
        // Création d'une nouvelle recette (multipart/form-data si image/file)
        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<ActionResult<RecipeDTO>> Create([FromForm] RecipeFormDto dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var created = await _service.AddRecipeAsync(dto, userId);

            // 201 Created + Location header
            return CreatedAtAction(
                nameof(GetById),
                new { id = created.RecipeId },
                created
            );
        }

        // GET /api/recipes/me
        // Récupérer les recettes du user courant
        [HttpGet("me")]
        public async Task<ActionResult<List<RecipeDTO>>> GetMyRecipes()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var list = await _service.GetRecipesByUserAsync(userId);
            return Ok(list);
        }

        // GET /api/recipes/{id}
        [HttpGet("{id:int}")]
        public async Task<ActionResult<RecipeDTO>> GetById(int id)
        {
            var dto = await _service.GetRecipeByIdAsync(id);
            if (dto == null) return NotFound();
            return Ok(dto);
        }

        // … vos autres endpoints POST, PUT, DELETE, etc.
    }
}

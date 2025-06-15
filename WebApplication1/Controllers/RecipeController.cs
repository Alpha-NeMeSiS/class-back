using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
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

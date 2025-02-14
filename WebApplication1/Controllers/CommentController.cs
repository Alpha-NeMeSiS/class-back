using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.DTO;
using WebApplication1.Models;
using WebApplication1.Repositories;
using WebApplication1.Service;

namespace WebApplication1.Controllers
{
    [Route("api/comments")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly CommentService _commentService;

        public CommentController(CommentService commentService)
        {
            _commentService = commentService;
        }

        // Récupérer les commentaires d'une recette
        [HttpGet("/api/recipes/{recipeId}/comments")]
        public async Task<IActionResult> GetCommentsByRecipe(int recipeId)
        {
            var comments = await _commentService.GetCommentsByRecipe(recipeId);
            return Ok(comments);
        }

        // Ajouter un commentaire (authentification requise)
        [HttpPost("/api/recipes/{recipeId}/comments")]
        //[Authorize]
        public async Task<IActionResult> AddComment(int recipeId, [FromBody] CommentDTO commentDto)
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (userId == null) return Unauthorized();

            var comment = await _commentService.AddComment(recipeId, userId, commentDto);
            return CreatedAtAction(nameof(GetCommentsByRecipe), new { recipeId = recipeId }, comment);
        }

        // Supprimer un commentaire (seulement par son auteur)
        [HttpDelete("{id}")]
        //[Authorize]
        public async Task<IActionResult> DeleteComment(int id)
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (userId == null) return Unauthorized();

            var result = await _commentService.DeleteComment(id, userId);
            if (!result) return NotFound(new { Message = "Commentaire non trouvé ou accès refusé" });

            return NoContent();
        }
    }
}

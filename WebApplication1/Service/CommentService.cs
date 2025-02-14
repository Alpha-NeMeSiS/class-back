using Microsoft.EntityFrameworkCore;
using WebApplication1.CourseDbContext;
using WebApplication1.DTO;
using WebApplication1.Models;

namespace WebApplication1.Service
{
    public class CommentService
    {
        private readonly ApplicationDbContext _context;

        public CommentService(ApplicationDbContext context)
        {
            _context = context;
        }

        //  Récupérer les commentaires d'une recette
        public async Task<IEnumerable<Comment>> GetCommentsByRecipe(int recipeId)
        {
            return await _context.Comments
                .Where(c => c.RecipeId == recipeId)
                .Include(c => c.User)
                .ToListAsync();
        }

        //  Ajouter un commentaire sur une recette
        public async Task<Comment> AddComment(int recipeId, string userId, CommentDTO commentDto)
        {
            var recipe = await _context.Recipes.FindAsync(recipeId);
            if (recipe == null)
                throw new Exception("Recette non trouvée.");

            var comment = new Comment
            {
                RecipeId = recipeId,
                UserId = userId,
                Content = commentDto.Content,
                Rating = commentDto.Rating,
                CreatedAt = DateTime.UtcNow
            };

            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();
            return comment;
        }

        //  Supprimer un commentaire (seulement par son auteur)
        public async Task<bool> DeleteComment(int commentId, string userId)
        {
            var comment = await _context.Comments.FindAsync(commentId);
            if (comment == null || comment.UserId != userId)
                return false;

            _context.Comments.Remove(comment);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}

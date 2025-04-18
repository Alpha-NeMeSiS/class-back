using Microsoft.EntityFrameworkCore;
using WebApplication1.CourseDbContext;
using WebApplication1.DTO;
using WebApplication1.Models;
using WebApplication1.Repositories;

namespace WebApplication1.Service
{
    public class CommentService
    {
        public CommentRepository _commentRepository { get; set; }

        public CommentService(CommentRepository commentRepository)
        {
            _commentRepository = commentRepository;
        }

        //  Récupérer les commentaires d'une recette
        public async Task<IEnumerable<CommentDTO>> GetCommentsByRecipe(int recipeId)
        {
            List<CommentDTO> commentsTOReturn = new List<CommentDTO>();
            IEnumerable<Comment> comments = await _commentRepository.GetCommentsByRecipe(recipeId);
            foreach(var comment in comments)
            {
                var cToAdd = new CommentDTO()
                {
                    CommentId = comment.CommentId,
                    Content = comment.Content,
                    Rating = comment.Rating,
                    RecipeId = comment.RecipeId
                };
                commentsTOReturn.Add(cToAdd);
            }
            return commentsTOReturn;
        }

        //  Ajouter un commentaire sur une recette
        public async Task<Comment> AddComment(int recipeId, string userId, CommentCreateDTO commentDto)
        {
            var recipe = await _commentRepository.GetCommentsByRecipe(recipeId);
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

            comment= await _commentRepository.AddComment(comment);

            return comment;
        }

        //  Supprimer un commentaire (seulement par son auteur)
        public async Task<bool> DeleteComment(int commentId, string userId)
        {

             var comment = await _commentRepository.GetCommentById(commentId);
            if (comment == null || comment.UserId != userId)
                return false;

            await _commentRepository.DeleteComment(comment);
            return true;
        }
    }
}

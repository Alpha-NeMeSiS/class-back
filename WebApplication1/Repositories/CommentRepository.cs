using Microsoft.EntityFrameworkCore;
using WebApplication1.CourseDbContext;
using WebApplication1.DTO;
using WebApplication1.Models;

namespace WebApplication1.Repositories
{
    public class CommentRepository
    {
        private readonly ApplicationDbContext _context;

        public CommentRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Comment>> GetCommentsByRecipe(int recipeId)
        {
            return await _context.Comments
                .Where(c => c.RecipeId == recipeId)
                .Include(c => c.User)
                .ToListAsync();
        }

        public async Task<Comment> AddComment(Comment comment)
        {
            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();
            return comment;
        }

        public async Task<bool> DeleteComment(Comment comment)
        {
            _context.Comments.Remove(comment);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
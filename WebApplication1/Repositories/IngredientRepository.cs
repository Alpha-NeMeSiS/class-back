using Microsoft.EntityFrameworkCore;
using WebApplication1.CourseDbContext;
using WebApplication1.Models;

namespace WebApplication1.Repositories
{
    public class IngredientRepository
    {
        private readonly ApplicationDbContext _context;

        public IngredientRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Ingredient>> GetIngredientByRecipeId(int id)
        {
            return await _context.Ingredients.Where(i => i.IngredientId == id).ToListAsync();
        }
    }
}

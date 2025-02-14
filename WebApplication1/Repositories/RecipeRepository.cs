using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using WebApplication1.CourseDbContext;
using WebApplication1.DTO;
using WebApplication1.Models;

namespace WebApplication1.Repositories
{
    public class RecipeRepository
    {
        private readonly ApplicationDbContext _context;

        public RecipeRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Recipe>> GetAllRecipes()
        {
            return await _context.Recipes
                .Include(r => r.Ingredients)
                .Include(r => r.Steps)
                .Include(r => r.Comments)
                .ToListAsync();
        }

        public async Task<Recipe?> GetRecipeById(int id)
        {
            return await _context.Recipes
                .Include(r => r.Ingredients)
                .Include(r => r.Steps)
                .Include(r => r.Comments)
                .FirstOrDefaultAsync(r => r.RecipeId == id);
        }

        public async Task<Recipe> CreateRecipe(Recipe recipe)
        {
            _context.Recipes.Add(recipe);
            await _context.SaveChangesAsync();
            return recipe;
        }

        public async Task<bool> UpdateRecipe(Recipe recipe)
        {
            _context.Recipes.Update(recipe);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteRecipe(Recipe recipe)
        {
            _context.Recipes.Remove(recipe);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}

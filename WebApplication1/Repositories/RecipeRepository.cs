using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.CourseDbContext;
using WebApplication1.Models;

namespace WebApplication1.Repositories
{
    public class RecipeRepository
    {
        private readonly ApplicationDbContext _context;

        public RecipeRepository(ApplicationDbContext context)
            => _context = context;

        /// <summary>
        /// Récupère toutes les recettes (avec ingrédients, étapes et commentaires)
        /// </summary>
        public async Task<List<Recipe>> GetAllAsync()
            => await _context.Recipes
                .Include(r => r.Ingredients)
                .Include(r => r.Steps)
                .Include(r => r.Comments)
                .ToListAsync();

        /// <summary>
        /// Recherche les recettes dont le titre ou la description contient la chaîne donnée
        /// </summary>
        public async Task<List<Recipe>> SearchAsync(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
                return new List<Recipe>();

            return await _context.Recipes
                .Where(r =>
                    EF.Functions.Like(r.Title, $"%{query}%") ||
                    EF.Functions.Like(r.Description, $"%{query}%"))
                .Include(r => r.Ingredients)
                .Include(r => r.Steps)
                .Include(r => r.Comments)
                .ToListAsync();
        }

        /// <summary>
        /// Récupère une recette par son ID (avec détails)
        /// </summary>
        public async Task<Recipe?> GetByIdAsync(int id)
            => await _context.Recipes
                .Include(r => r.Ingredients)
                .Include(r => r.Steps)
                .Include(r => r.Comments)
                .FirstOrDefaultAsync(r => r.RecipeId == id);

        /// <summary>
        /// Récupère toutes les recettes créées par un utilisateur
        /// </summary>
        public async Task<List<Recipe>> GetByUserAsync(string userId)
            => await _context.Recipes
                .Where(r => r.CreatedBy == userId)
                .Include(r => r.Ingredients)
                .Include(r => r.Steps)
                .Include(r => r.Comments)
                .ToListAsync();

        /// <summary>
        /// Ajoute une nouvelle recette et renvoie l’entité créée (avec son ID)
        /// </summary>
        public async Task<Recipe> AddAsync(Recipe recipe)
        {
            _context.Recipes.Add(recipe);
            await _context.SaveChangesAsync();
            return recipe;
        }

        /// <summary>
        /// Met à jour une recette existante
        /// </summary>
        public async Task UpdateAsync(Recipe recipe)
        {
            _context.Recipes.Update(recipe);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Supprime une recette si le userId correspond au créateur
        /// </summary>
        public async Task<bool> DeleteAsync(int id, string userId)
        {
            var recipe = await _context.Recipes.FindAsync(id);
            if (recipe == null || recipe.CreatedBy != userId)
                return false;

            _context.Recipes.Remove(recipe);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}

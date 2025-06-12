// Repositories/RecipeRepository.cs
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.CourseDbContext;
using WebApplication1.Models;
using WebApplication1.Service;

namespace WebApplication1.Repositories
{
    // Veillez à déclarer également cette interface en public dans IRecipeRepository.cs
    public class RecipeRepository : IRecipeRepository
    {
        private readonly ApplicationDbContext _context;

        public RecipeRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        // Récupère toutes les recettes (inclut ingrédients, étapes, commentaires)
        public async Task<List<Recipe>> GetAllAsync()
        {
            return await _context.Recipes
                .Include(r => r.Ingredients)
                .Include(r => r.Steps)
                .Include(r => r.Comments)
                .ToListAsync();
        }

        // Récupère une recette par son ID
        public async Task<Recipe?> GetByIdAsync(int id)
        {
            return await _context.Recipes
                .Include(r => r.Ingredients)
                .Include(r => r.Steps)
                .Include(r => r.Comments)
                .FirstOrDefaultAsync(r => r.RecipeId == id);
        }

        // Ajoute une nouvelle recette et renvoie l’entité avec son ID
        public async Task<Recipe> AddAsync(Recipe recipe)
        {
            _context.Recipes.Add(recipe);
            await _context.SaveChangesAsync();
            return recipe;
        }

        // Met à jour une recette existante
        public async Task UpdateAsync(Recipe recipe)
        {
            _context.Recipes.Update(recipe);
            await _context.SaveChangesAsync();
        }

        // Supprime une recette, uniquement si userId correspond au créateur
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

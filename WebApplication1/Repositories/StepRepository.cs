using Microsoft.EntityFrameworkCore;
using WebApplication1.CourseDbContext;
using WebApplication1.Models;

namespace WebApplication1.Repositories
{
    public class StepRepository
    {
        private readonly ApplicationDbContext _context;

        public StepRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Step>> GetStepByRecipeId(int id)
        {
            return await _context.Steps.Where(s => s.StepId == id).ToListAsync();
        }
    }
}

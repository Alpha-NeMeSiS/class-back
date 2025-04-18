using System.ComponentModel.DataAnnotations.Schema;
using WebApplication1.Models;

namespace WebApplication1.DTO
{
    public class RecipeDTO
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public int PreparationTime { get; set; } // en minutes
        public int CookingTime { get; set; } // en minutes
        public string Difficulty { get; set; } // Facile, Moyen, Difficile
        public string Budget { get; set; } // Économique, Moyen, Cher
        public string DietType { get; set; } // Végétarien, Sans Gluten, etc.
        public string UserId { get; set; } 

        public List<IngredientDTO> Ingredients { get; set; } = new List<IngredientDTO>();
        public List<StepDTO> Steps { get; set; } = new List<StepDTO>();
    }
}
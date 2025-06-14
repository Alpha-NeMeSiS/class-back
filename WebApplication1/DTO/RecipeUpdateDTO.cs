using System.Collections.Generic;

namespace WebApplication1.DTO
{
    public class RecipeUpdateDTO
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public int PreparationTime { get; set; }      // en minutes
        public int CookingTime { get; set; }          // en minutes

        // Nouveaux champs :
        public int Servings { get; set; }
        public string Category { get; set; }
        public string ImageUrl { get; set; }

        public string UserId { get; set; }

        public List<IngredientDTO> Ingredients { get; set; } = new List<IngredientDTO>();
        public List<StepDTO> Steps { get; set; } = new List<StepDTO>();
    }
}

using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1.DTO
{
    public class RecipeFormDto
    {
        [Required] public string Title { get; set; }
        public string Description { get; set; }

        [Range(0, int.MaxValue)] public int PreparationTime { get; set; }
        [Range(0, int.MaxValue)] public int CookingTime { get; set; }
        [Range(1, int.MaxValue)] public int Servings { get; set; }

        public string Category { get; set; }
        public IFormFile Image { get; set; }

        // on ajoute cette propriété pour recevoir les unités
        public string[] IngredientUnits { get; set; }

        // déjà existant : réceptionne plusieurs fois data.append("ingredients", name)
        public string[] Ingredients { get; set; }

        public string[] Instructions { get; set; }

        [Required] public string UserId { get; set; }
    }
}

using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1.DTO
{
    /// <summary>
    /// DTO utilisé pour la création et l’édition d’une recette
    /// </summary>
    public class RecipeFormDto
    {
        [Required] public string Title { get; set; }
        public string Description { get; set; }

        [Range(0, int.MaxValue)] public int PreparationTime { get; set; }
        [Range(0, int.MaxValue)] public int CookingTime { get; set; }
        [Range(1, int.MaxValue)] public int Servings { get; set; }

        public string Category { get; set; }

        [Required]
        public IFormFile Image { get; set; }

        // réception des quantités/unités via form data
        public string[] IngredientUnits { get; set; }
        public string[] Ingredients { get; set; }
        public string[] Instructions { get; set; }

        [Required] public string UserId { get; set; }
    }
}

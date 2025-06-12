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

        // Le fichier image envoyé par front
        public IFormFile Image { get; set; }

        // front fait `data.append("ingredients", item)` plusieurs fois
        public string[] Ingredients { get; set; }

        // front fait `data.append("instructions", item)` plusieurs fois
        public string[] Instructions { get; set; }

        // pour gérer la liaison à l’utilisateur courant si vous l’envoyez
        public string UserId { get; set; }
    }
}

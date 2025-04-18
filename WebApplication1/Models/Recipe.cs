using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace WebApplication1.Models
{
        public class Recipe
        {
            [Key]
            public int RecipeId { get; set; }

            [Required, MaxLength(100)]
            public string Title { get; set; }

            public string Description { get; set; }

            [Required]
            public int PreparationTime { get; set; } // en minutes

            [Required]
            public int CookingTime { get; set; } // en minutes

            [Required]
            public string Difficulty { get; set; } // Facile, Moyen, Difficile

            public string Budget { get; set; } // Économique, Moyen, Cher

            public string DietType { get; set; } // Végétarien, Sans Gluten, etc.

           //Lite des ingrédients
            public List<Ingredient> Ingredients { get; set; } = new List<Ingredient>();

            //Lite des étapes
            public List<Step> Steps { get; set; } = new List<Step>();

            //Lite des commentaires
            public List<Comment> Comments { get; set; } = new List<Comment>();

            [ForeignKey("UserId")]
            public string CreatedBy { get; set; } // UserId du créateur

        }
}

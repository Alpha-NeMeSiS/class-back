using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class Step
    {
        [Key]
        public int StepId { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public int Order { get; set; } // Ordre de l'étape

        // Relation avec Recipe
        [ForeignKey("Recipe")]
        public int RecipeId { get; set; }
        public Recipe Recipe { get; set; }
    }
}

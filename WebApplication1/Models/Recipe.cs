using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebApplication1.Models;

public class Recipe
{
    [Key]
    public int RecipeId { get; set; }

    [Required, MaxLength(100)]
    public string Title { get; set; }

    public string Description { get; set; }

    [Required]
    public int PreparationTime { get; set; }

    [Required]
    public int CookingTime { get; set; }

    // ← Nouveau
    [Required]
    public int Servings { get; set; }

    // ← Nouveau
    [MaxLength(50)]
    public string Category { get; set; }

    // ← Nouveau : URL du fichier uploadé
    public string ImageUrl { get; set; }

    public string Difficulty { get; set; }
    public string Budget { get; set; }
    public string DietType { get; set; }

    public List<Ingredient> Ingredients { get; set; } = new();
    public List<Step> Steps { get; set; } = new();
    public List<Comment> Comments { get; set; } = new();

    [ForeignKey("UserId")]
    public string CreatedBy { get; set; }
}

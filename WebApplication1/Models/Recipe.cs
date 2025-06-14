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

    
    [Required]
    public int Servings { get; set; }

   
    [MaxLength(50)]
    public string Category { get; set; }

    
    public string ImageUrl { get; set; }

    public List<Ingredient> Ingredients { get; set; } = new();
    public List<Step> Steps { get; set; } = new();
    public List<Comment> Comments { get; set; } = new();

    [ForeignKey("UserId")]
    public string CreatedBy { get; set; }
}

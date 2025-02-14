using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models
{
    public class Comment
    {
        [Key]
        public int CommentId { get; set; }

        [Required]
        public string Content { get; set; }

        [Required]
        public int Rating { get; set; } // Note entre 1 et 5

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Relation avec Recipe
        [ForeignKey("Recipe")]
        public int RecipeId { get; set; }
        public Recipe Recipe { get; set; }

        // Relation avec User
        [ForeignKey("User")]
        public string UserId { get; set; }
        public User User { get; set; }
    }
}
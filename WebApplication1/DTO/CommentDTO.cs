namespace WebApplication1.DTO
{   public class CommentDTO
    {
            public int CommentId { get; set; }
            public string Content { get; set; }
            public int Rating { get; set; } // Note entre 1 et 5
            
            public int RecipeId { get; set; }
    }
}

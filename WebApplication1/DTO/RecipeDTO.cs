namespace WebApplication1.DTO
{
    public class RecipeDTO
    {
        public int RecipeId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int PreparationTime { get; set; }
        public int CookingTime { get; set; }
        public int Servings { get; set; }
        public string Category { get; set; }
        public string ImageUrl { get; set; }
        public string UserId { get; set; }

        public List<IngredientDTO> Ingredients { get; set; }  // maintenant avec Unit
        public List<StepDTO> Steps { get; set; }
    }
}

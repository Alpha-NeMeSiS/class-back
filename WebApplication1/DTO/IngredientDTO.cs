namespace WebApplication1.DTO
{
    public class IngredientDTO
    {
            public int IngredientId { get; set; }
            public string Name { get; set; }
            public float Quantity { get; set; } // Quantité de l'ingrédient
            public string Unit { get; set; } // g, ml, unité, etc.
 
    }

}
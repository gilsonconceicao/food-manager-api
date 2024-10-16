using System.ComponentModel;

namespace FoodManager.Domain.Enums
{
    public enum FoodCategoryEnum
    {
        [Description("Entrada")]
        Appetizer = 0,
        
        [Description("Prato Principal")]
        MainCourse = 1,
        
        [Description("Sobremesa")]
        Dessert = 2,
        
        [Description("Bebida")]
        Beverage = 3,
        
        [Description("Salada")]
        Salad = 4,
        
        [Description("Sopa")]
        Soup = 5,
        
        [Description("Lanche")]
        Snack = 6, 
        
        [Description("Doce")]
        Sweet = 7,

        [Description("Fruta")]
        Fruit = 8,

        [Description("Massa")]
        Pasta = 9,

        [Description("Pizza")]
        Pizza = 10,

        [Description("Sushi")]
        Sushi = 11,

        [Description("Carne")]
        Meat = 12,

        [Description("Peixe")]
        Fish = 13,

        [Description("Mariscos")]
        Seafood = 14,

        [Description("Fast Food")]
        FastFood = 15,

        [Description("Vegetariano")]
        Vegetarian = 16,

        [Description("Vegano")]
        Vegan = 17
    }
}

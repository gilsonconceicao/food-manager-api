using System.ComponentModel;

namespace FoodManager.Domain.Enums
{
    public enum FoodCategory
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
        Sweet = 7
    }
}

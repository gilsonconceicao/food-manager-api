using Domain.Enums;
#nullable disable
namespace Application.Foods.Queries.GetAllWithPaginationFoodQuery
{
    public class FoodItemsDto
    {
        public string Name { get; set; }
        public string Url { get; set; }
        public decimal Price { get; set; }
        public string CategoryDisplay { get; set; }
        public FoodCategoryEnum? Category { get; set; }
    }
}
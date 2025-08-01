#nullable disable
namespace Application.Foods.Queries.GetAllWithPaginationFoodQuery
{
    public class FoodDto : BaseModelResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Url { get; set; }
        public decimal Price { get; set; }
        public string CategoryDisplay { get; set; }
        public string Category { get; set; }
    }
}
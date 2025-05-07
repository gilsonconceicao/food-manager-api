using Domain.Enums;

namespace Application.Foods.Commands.Dtos;
#nullable disable
public class FoodUpdateDto
{
    public string Name { get; set; }
    public string Url { get; set; }
    public string Description { get; set; }
    public bool IsAvailable { get; set; }
    public decimal Price { get; set; }
    public FoodCategoryEnum Category { get; set; }
}
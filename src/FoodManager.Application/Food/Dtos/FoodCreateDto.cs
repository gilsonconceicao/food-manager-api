using FoodManager.Domain.Enums;

namespace FoodManager.Application.Foods.Commands.Dtos;
#nullable disable
public class FoodCreateDto
{
    public string Name { get; set; }
    public string UrlImage { get; set; }
    public string Description { get; set; }
    public bool IsAvailable { get; set; }
    public decimal Price { get; set; }
    public FoodCategoryEnum Category { get; set; }
}
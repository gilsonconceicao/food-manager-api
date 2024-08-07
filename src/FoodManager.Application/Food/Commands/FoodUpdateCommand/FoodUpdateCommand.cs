using FoodManager.Domain.Enums;
using MediatR;

namespace FoodManager.Application.Foods.Commands.FoodUpdateCommand;

public class FoodUpdateCommand : IRequest<bool>
{
    public FoodUpdateCommand(
        Guid Id,
        string Name,
        string UrlImage,
        string Description,
        bool IsAvailable,
        decimal Price,
        FoodCategoryEnum Category,
        string PreparationTime)
    {
        this.Id = Id;
        this.Name = Name;
        this.UrlImage = UrlImage;
        this.Description = Description;
        this.IsAvailable = IsAvailable;
        this.Price = Price;
        this.Category = Category;
        this.PreparationTime = PreparationTime;
    }
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string UrlImage { get; set; }
    public string Description { get; set; }
    public bool IsAvailable { get; set; }
    public decimal Price { get; set; }
    public FoodCategoryEnum Category { get; set; }
    public string PreparationTime { get; set; }
}
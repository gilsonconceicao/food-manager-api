using FoodManager.Domain.Enums;
using MediatR;
#nullable disable
namespace FoodManager.Application.Foods.Commands.CreateFoodCommand
{
    public class CreateFoodCommand : IRequest<bool>
    {
        public string Name { get; set; }
        public string UrlImage { get; set; }
        public string Description { get; set; }
        public bool IsAvailable { get; set; }
        public decimal Price { get; set; }
        public FoodCategory Category { get; set; }
        public string PreparationTime { get; set; }
    }
}
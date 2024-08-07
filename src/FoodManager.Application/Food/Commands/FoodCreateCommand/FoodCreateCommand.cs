using FoodManager.Domain.Enums;
using MediatR;
#nullable disable
namespace FoodManager.Application.Foods.Commands.FoodCreateCommand
{
    public class FoodCreateCommand : IRequest<bool>
    {
        public string Name { get; set; }
        public string UrlImage { get; set; }
        public string Description { get; set; }
        public bool IsAvailable { get; set; }
        public decimal Price { get; set; }
        public FoodCategoryEnum Category { get; set; }
        public string PreparationTime { get; set; }
    }
}
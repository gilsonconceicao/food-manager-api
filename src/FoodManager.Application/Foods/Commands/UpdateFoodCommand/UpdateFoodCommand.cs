using FoodManager.Application.Foods.Commands.Dtos;
using FoodManager.Domain.Enums;
using MediatR;

namespace FoodManager.Application.Foods.Commands.UpdateFoodCommand;

public class UpdateFoodCommand : IRequest<bool>
{
    public Guid Id { get; set; }
    public FoodUpdateDto Payload { get; set; }
    public UpdateFoodCommand(Guid id, FoodUpdateDto payload)
    {
        this.Id = id;
        this.Payload = payload; 
    }
}
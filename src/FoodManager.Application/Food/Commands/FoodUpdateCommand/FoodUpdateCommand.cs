using FoodManager.Application.Foods.Commands.Dtos;
using FoodManager.Domain.Enums;
using MediatR;

namespace FoodManager.Application.Foods.Commands.FoodUpdateCommand;

public class FoodUpdateCommand : IRequest<bool>
{
    public Guid Id { get; set; }
    public FoodUpdateDto Payload { get; set; }
    public FoodUpdateCommand(Guid id, FoodUpdateDto payload)
    {
        this.Id = id;
        this.Payload = payload; 
    }
}
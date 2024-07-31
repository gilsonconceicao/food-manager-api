using MediatR;

namespace FoodManager.Application.Foods.Commands.FoodDeleteCommand; 

public class FoodDeleteCommand : IRequest<bool> 
{
    public Guid Id { get; set; }
    public FoodDeleteCommand(Guid id)
    {
        this.Id = id;
    }
}
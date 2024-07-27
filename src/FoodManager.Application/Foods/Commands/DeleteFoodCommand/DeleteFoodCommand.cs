using MediatR;

namespace FoodManager.Application.Foods.Commands.DeleteFoodCommand; 

public class DeleteFoodCommand : IRequest<bool> 
{
    public Guid Id { get; set; }
    public DeleteFoodCommand(Guid id)
    {
        this.Id = id;
    }
}
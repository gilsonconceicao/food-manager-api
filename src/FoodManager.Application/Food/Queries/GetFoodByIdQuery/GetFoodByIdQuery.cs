using FoodManager.Domain.Models;
using MediatR;

#nullable disable
namespace FoodManager.Application.Foods.Queries.GetFoodByIdQuery
{
    public class GetFoodByIdQuery : IRequest<Food>
    {
        public Guid Id { get; set;}
        public GetFoodByIdQuery(Guid id)
        {
            this.Id = id;
        }
    }
}
using FoodManager.Application.Foods.Queries.GetAllWithPaginationFoodQuery;
using MediatR;

namespace FoodManager.Application.Foods.Queries.GetFoodByIdQuery
{
    public class GetFoodByIdQuery : IRequest<GetFoodModel>
    {
        public Guid Id { get; set;}
        public GetFoodByIdQuery(Guid id)
        {
            this.Id = id;
        }
    }
}
using FoodManager.Domain.Extensions;
using MediatR;

namespace FoodManager.Application.Foods.Queries.GetAllWithPaginationFoodQuery
{
    public class GetAllWithPaginationFoodQuery : IRequest<PagedList<GetFoodModel>>
    {
        public int Page { get; set; } = 0;
        public int Size { get; set; } = 5;
    }
}
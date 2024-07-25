using FoodManager.Domain.Extensions;
using MediatR;

namespace FoodManager.Application.Foods.Queries.GetAllWithPaginationFoodQuery
{
    public class GetAllWithPaginationFoodQuery : IRequest<PagedList<GetAllWithPaginationModel>>
    {
        public int Page { get; set; } = 0;
        public int Size { get; set; } = 5;
    }
}
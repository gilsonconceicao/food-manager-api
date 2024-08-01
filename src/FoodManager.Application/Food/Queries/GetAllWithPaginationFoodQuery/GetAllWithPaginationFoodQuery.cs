using FoodManager.Domain.Extensions;
using FoodManager.Domain.Models;
using MediatR;

namespace FoodManager.Application.Foods.Queries.GetAllWithPaginationFoodQuery
{
    public class GetAllWithPaginationFoodQuery : IRequest<ListDataResponse<List<Food>>>
    {
        public int Page { get; set; } = 0;
        public int Size { get; set; } = 5;
    }
}
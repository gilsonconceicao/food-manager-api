

using AutoMapper;
using Domain.Extensions;
using Domain.Models;
using Infrastructure.Database;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Foods.Queries.FoodGetListPaginationQuery
{
    public class FoodGetListPaginationQuery : IRequest<ListDataResponse<List<Food>>>
    {
        public int Page { get; set; } = 0;
        public int Size { get; set; } = 5;
    }
    public class FoodGetListPaginationQueryHandler : IRequestHandler<FoodGetListPaginationQuery, ListDataResponse<List<Food>>>
    {
        private readonly DataBaseContext _context;
        private readonly IMapper _mapper;
        public FoodGetListPaginationQueryHandler(DataBaseContext context,
            IMapper mapper
        )
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ListDataResponse<List<Food>>> Handle(FoodGetListPaginationQuery request, CancellationToken cancellationToken)
        {
            var page = request.Page;
            var size = request.Size;

            var queryData = _context.Foods
                .Include(x => x.Items)
                .ThenInclude(x => x.Order)
                .Where(x => !x.IsDeleted)
                .OrderBy(c => c.Name);

            var totalCount = await queryData.CountAsync(cancellationToken);

            var data = await queryData
                .Skip((page) * size)
                .Take(size)
                .ToListAsync(cancellationToken);

            return new ListDataResponse<List<Food>>
            {
                Count = totalCount,
                Data = data
            };
        }

    }
}
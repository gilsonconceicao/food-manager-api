

using AutoMapper;
using FoodManager.Domain.Extensions;
using FoodManager.Infrastructure.Database;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FoodManager.Application.Foods.Queries.GetAllWithPaginationFoodQuery
{
    public class GetAllWithPaginationFoodHandler : IRequestHandler<GetAllWithPaginationFoodQuery, PagedList<GetAllWithPaginationModel>>
    {
        private readonly DataBaseContext _context;
        private readonly IMapper _mapper;
        public GetAllWithPaginationFoodHandler(DataBaseContext context,
            IMapper mapper
        )
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<PagedList<GetAllWithPaginationModel>> Handle(GetAllWithPaginationFoodQuery request, CancellationToken cancellationToken)
        {
            var page = request.Page;
            var size = request.Size;

            var queryData = _context.Foods.Where(x => !x.IsDeleted);

            var totalCount = await queryData.CountAsync(cancellationToken);

            var listPaginated = await queryData
                .Skip((page) * size)
                .Take(size)
                .ToListAsync(cancellationToken);

            var foodList = _mapper.Map<List<GetAllWithPaginationModel>>(listPaginated);

            var dataMapped = new PagedList<GetAllWithPaginationModel>(foodList, totalCount, request.Page, request.Size);

            return dataMapped;
        }
    }
}
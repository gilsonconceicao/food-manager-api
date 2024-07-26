

using AutoMapper;
using FoodManager.Application.Common.Exceptions;
using FoodManager.Domain.Extensions;
using FoodManager.Infrastructure.Database;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FoodManager.Application.Foods.Queries.GetAllWithPaginationFoodQuery
{
    public class GetAllWithPaginationFoodHandler : IRequestHandler<GetAllWithPaginationFoodQuery, PagedList<GetFoodModel>>
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

        public async Task<PagedList<GetFoodModel>> Handle(GetAllWithPaginationFoodQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var page = request.Page;
                var size = request.Size;

                var queryData = _context.Foods.Where(x => !x.IsDeleted);

                var totalCount = await queryData.CountAsync(cancellationToken);

                var listPaginated = await queryData
                    .Skip((page) * size)
                    .Take(size)
                    .ToListAsync(cancellationToken);

                var foodList = _mapper.Map<List<GetFoodModel>>(listPaginated);

                var dataMapped = new PagedList<GetFoodModel>(foodList, totalCount, request.Page, request.Size);

                return dataMapped;
            }
            catch (Exception ex)
            {
                throw new HttpResponseException
                {
                    Status = 500,
                    Value = new
                    {
                        Message = ex.Message,
                    }
                };
            }
        }
    }
}
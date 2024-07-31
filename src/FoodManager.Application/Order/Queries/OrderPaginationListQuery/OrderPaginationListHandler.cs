using AutoMapper;
using FoodManager.Application.Orders.Dtos;
using FoodManager.Domain.Extensions;
using FoodManager.Domain.Models;
using FoodManager.Infrastructure.Database;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FoodManager.Application.Orders.Queries.OrderPaginationListQuery;

public class OrderPaginationListHandler : IRequestHandler<OrderPaginationListQuery, PagedList<OrderGetDto>>
{
    private readonly DataBaseContext _context;
    private readonly IMapper _mapper;
    
    public OrderPaginationListHandler(DataBaseContext context,
    IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PagedList<OrderGetDto>> Handle(OrderPaginationListQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var page = request.Page;
            var size = request.Size;

            var queryData = _context
                .Orders
                .Include(x => x.Client)
                .Where(x => !x.IsDeleted);

            var totalCount = await queryData.CountAsync(cancellationToken);

            var listPaginated = await queryData
                .Skip((page) * size)
                .Take(size)
                .ToListAsync(cancellationToken);

            listPaginated.OrderByDescending(x => x.CreatedAt);

            var listMapped = _mapper.Map<List<OrderGetDto>>(listPaginated);

            return new PagedList<OrderGetDto>(
                data: listMapped, 
                count: totalCount, 
                pageNumber: request.Page, 
                pageSize: request.Size
            );
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }
}

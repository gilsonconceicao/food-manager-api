using AutoMapper;
using FoodManager.Application.Orders.Dtos;
using FoodManager.Domain.Extensions;
using FoodManager.Domain.Models;
using FoodManager.Infrastructure.Database;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FoodManager.Application.Orders.Queries.OrderPaginationListQuery;

public class OrderPaginationListHandler : IRequestHandler<OrderPaginationListQuery, ListDataResponse<List<Order>>>
{
    private readonly DataBaseContext _context;
    private readonly IMapper _mapper;
    
    public OrderPaginationListHandler(DataBaseContext context,
    IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<ListDataResponse<List<Order>>> Handle(OrderPaginationListQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var page = request.Page;
            var size = request.Size;

            var queryData = _context
                .Orders
                .Include(x => x.Client)
                .Include(x => x.OrdersFoodsRelationship)
                .ThenInclude(x => x.Food)
                .Where(x => !x.IsDeleted);

            var totalCount = await queryData.CountAsync(cancellationToken);

            var data = await queryData
                .Skip((page) * size)
                .Take(size)
                .ToListAsync(cancellationToken);

            data.OrderByDescending(x => x.CreatedAt);

            return new ListDataResponse<List<Order>> 
            {
                Count = totalCount,
                Data = data
            };
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }
}

using Api.Services;
using AutoMapper;
using Domain.Extensions;
using Domain.Models;
using Infrastructure.Database;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Orders.Queries;

public class OrderPaginationListQuery : IRequest<ListDataResponse<List<Order>>>
{
    public int Page { get; set; } = 0;
    public int Size { get; set; } = 5;
}
public class OrderPaginationListHandler : IRequestHandler<OrderPaginationListQuery, ListDataResponse<List<Order>>>
{
    private readonly DataBaseContext _context;
    private readonly ICurrentUser _httpUserService;


    public OrderPaginationListHandler(DataBaseContext context, ICurrentUser currentUser)
    {
        _context = context;
        _httpUserService = currentUser;
    }

    public async Task<ListDataResponse<List<Order>>> Handle(OrderPaginationListQuery request, CancellationToken cancellationToken)
    {
        var user = await _httpUserService.GetAuthenticatedUser();

        var page = request.Page;
        var size = request.Size;

        var queryData = _context
            .Orders
            .Include(x => x.User)
            .Include(x => x.Items)
            .ThenInclude(x => x.Food)
            .Where(x => !x.IsDeleted && x.CreatedByUserId == user.UserId); 

        var totalCount = await queryData.CountAsync(cancellationToken);

        var data = await queryData
            .Skip((page) * size)
            .Take(size)
            .OrderByDescending(c => c.UpdatedAt != null ? c.UpdatedAt : c.CreatedAt)
            .ToListAsync(cancellationToken);

        return new ListDataResponse<List<Order>>
        {
            Count = totalCount,
            Data = data
        };

    }
}

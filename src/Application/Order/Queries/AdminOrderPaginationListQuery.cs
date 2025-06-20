using Domain.Extensions;
using Domain.Models;
using Infrastructure.Database;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Orders.Queries;

public class AdminOrderPaginationListQuery : IRequest<ListDataResponse<List<Order>>>
{
    public int Page { get; set; } = 0;
    public int Size { get; set; } = 5;
}
public class AdminOrderPaginationListQueryHandler : IRequestHandler<AdminOrderPaginationListQuery, ListDataResponse<List<Order>>>
{
    private readonly DataBaseContext _context;

    public AdminOrderPaginationListQueryHandler(DataBaseContext context)
    {
        _context = context;
    }

    public async Task<ListDataResponse<List<Order>>> Handle(AdminOrderPaginationListQuery request, CancellationToken cancellationToken)
    {
        var page = request.Page;
        var size = request.Size;

        var queryData = _context
            .Orders
            .Include(x => x.User)
            .Include(x => x.Items)
            .ThenInclude(x => x.Food)
            .Where(x => !x.IsDeleted);

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

using FoodManager.Domain.Extensions;
using FoodManager.Domain.Models;
using FoodManager.Infrastructure.Database;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FoodManager.Application.Users.Queries;

public class UserPaginationListQuery : IRequest<ListDataResponse<List<User>>>
{
    public int Page { get; set; } = 0;
    public int Size { get; set; } = 5;
}

public class UserPaginationListQueryHandler : IRequestHandler<UserPaginationListQuery, ListDataResponse<List<User>>>
{
    private readonly DataBaseContext _context;

    public UserPaginationListQueryHandler(DataBaseContext context)
    {
        _context = context;
    }
    public async Task<ListDataResponse<List<User>>> Handle(UserPaginationListQuery request, CancellationToken cancellationToken)
    {
        var page = request.Page;
        var size = request.Size;

        var queryData = _context
            .Users
            .Include(x => x.Address)
            .Include(x => x.Orders)
            .Where(x => !x.IsDeleted)
            .Skip((page) * size)
            .Take(size);

        queryData.OrderByDescending(x => x.CreatedAt);
        var totalCount = await queryData.CountAsync(cancellationToken);
        var data = await queryData.ToListAsync(); 

        
        return new ListDataResponse<List<User>>
        {
            Count = totalCount,
            Data = data
        };
    }
}
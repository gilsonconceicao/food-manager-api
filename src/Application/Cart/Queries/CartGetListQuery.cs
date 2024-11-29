

using Api.Services;
using Domain.Models;
using Infrastructure.Database;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Carts.Queries
{
#nullable disable
    public class CartGetListQuery : IRequest<List<Cart>> { }

    public class CartGetListQueryHandler : IRequestHandler<CartGetListQuery, List<Cart>>
    {
        private readonly DataBaseContext _context;
        private readonly IHttpUserService _httpUserService;

        public CartGetListQueryHandler(
            DataBaseContext context,
            IHttpUserService httpUserService
        )
        {
            _context = context;
            _httpUserService = httpUserService;
        }

        public async Task<List<Cart>> Handle(CartGetListQuery request, CancellationToken cancellationToken)
        {
            var user = await _httpUserService.GetAuthenticatedUser();
            return await _context.Carts
                .Include(x => x.Food)
                .Where(c => !c.IsDeleted)
                .Where(c => c.CreatedByUserId == user.UserId)
                .ToListAsync();
        }
    }
}

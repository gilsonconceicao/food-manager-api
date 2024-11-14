

using FoodManager.Application.Common.Exceptions;
using FoodManager.Domain.Models;
using FoodManager.Infrastructure.Database;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FoodManager.Application.Carts.Queries
{
    public class CartGetListQuery : IRequest<List<Cart>> { }

    public class CartGetListQueryHandler : IRequestHandler<CartGetListQuery, List<Cart>>
    {
        private readonly DataBaseContext _context;
        
        public CartGetListQueryHandler(DataBaseContext context)
        {
            _context = context;
        }

        public async Task<List<Cart>> Handle(CartGetListQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var carts = await _context.Carts
                    .Where(c => !c.IsDeleted)
                    .ToListAsync();
                return carts;
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



using Api.Services;
using Application.Common.Exceptions;
using Domain.Models;
using Infrastructure.Database;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Carts.Queries
{
    #nullable disable
    public class CartGetListQuery : IRequest<List<Cart>> {}

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
            var user = await _httpUserService.getAuthenticatedUser();
            try
            {
                var carts = await _context.Carts
                    .Where(c => !c.IsDeleted)
                    .Where(c => c.CreatedByUserId == user.UserId)
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

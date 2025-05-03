

using Api.Services;
using Application.Carts.Dtos;
using AutoMapper;
using Infrastructure.Database;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Carts.Queries
{
#nullable disable
    public class CartGetListQuery : IRequest<CartListDto> { }

    public class CartGetListQueryHandler : IRequestHandler<CartGetListQuery, CartListDto>
    {
        private readonly DataBaseContext _context;
        private readonly ICurrentUser _httpUserService;
        private readonly IMapper _mapper;

        public CartGetListQueryHandler(
            DataBaseContext context,
            ICurrentUser httpUserService, 
            IMapper mapper
        )
        {
            _context = context;
            _httpUserService = httpUserService;
            _mapper = mapper;
        }

        public async Task<CartListDto> Handle(CartGetListQuery request, CancellationToken cancellationToken)
        {
            var user = await _httpUserService.GetAuthenticatedUser();

            var queryData = await _context.Carts
                .Include(x => x.Food)
                .AsNoTracking()
                .Where(c => !c.IsDeleted)
                .Where(c => c.CreatedByUserId == user.UserId)
                .OrderBy(c => c.CreatedAt)
                .ToListAsync(); 

            var titalItemss = queryData.Sum(x => x.Food.Price); 
            var list = queryData.Select(x => _mapper.Map<CartDto>(x)).ToList(); 

            return new()
            {
                Data = queryData.Select(x => _mapper.Map<CartDto>(x)).ToList(),
                Summary = new SummaryCartDto
                {
                    TotalValue = queryData.Sum(x => x.Food.Price * x.Quantity) ?? 0
                }
            };
        }
    }
}

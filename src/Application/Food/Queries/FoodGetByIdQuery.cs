using AutoMapper;
using Domain.Models;
using Infrastructure.Database;
using MediatR;
using Microsoft.EntityFrameworkCore;
#nullable disable

namespace Application.Foods.Queries.FoodGetByIdQuery
{
    public class FoodGetByIdQuery : IRequest<Food>
    {
        public Guid Id { get; set; }
    }
    public class FoodGetByIdQueryHandler : IRequestHandler<FoodGetByIdQuery, Food>
    {
        private readonly DataBaseContext _context;
        private readonly IMapper _mapper;
        public FoodGetByIdQueryHandler(DataBaseContext context,
            IMapper mapper
        )
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Food> Handle(FoodGetByIdQuery request, CancellationToken cancellationToken)
        {
            return await _context.Foods
                .Include(x => x.Items)
                .ThenInclude(x => x.Order)
                .FirstOrDefaultAsync(x => x.Id == request.Id && !x.IsDeleted);
        }

    }
}
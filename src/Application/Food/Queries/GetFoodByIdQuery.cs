using AutoMapper;
using Api.Enums;
using Application.Common.Exceptions;
using Application.Foods.Queries.GetAllWithPaginationFoodQuery;
using Domain.Models;
using Infrastructure.Database;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Foods.Queries.GetFoodByIdQuery;

 public class GetFoodByIdQuery : IRequest<Food>
    {
        public Guid Id { get; set;}
        public GetFoodByIdQuery(Guid id)
        {
            this.Id = id;
        }
    }

public class GetFoodByIdHandler : IRequestHandler<GetFoodByIdQuery, Food>
{
    private readonly DataBaseContext _context;
    private readonly IMapper _mapper;
    public GetFoodByIdHandler(DataBaseContext context,
    IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<Food> Handle(GetFoodByIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var getFoodById = await _context
               .Items
               .Include(x => x.Order)
               .Include(x => x.Food)
               .Where(x => !x.Food.IsDeleted)
               .FirstOrDefaultAsync(x => x.Food.Id == request.Id) 
               ?? throw new HttpResponseException
                {
                    Status = 404,
                    Value = new
                    {
                        Code = CodeErrorEnum.NOT_FOUND_RESOURCE.ToString(),
                        Message = "Comida não encontrada ou não existe",
                    }
                };

            return getFoodById.Food; 
        }
        catch (HttpResponseException)
        {
            throw;
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
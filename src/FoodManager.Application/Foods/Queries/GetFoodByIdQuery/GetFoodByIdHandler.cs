using AutoMapper;
using FoodManager.API.Enums;
using FoodManager.Application.Common.Exceptions;
using FoodManager.Application.Foods.Queries.GetAllWithPaginationFoodQuery;
using FoodManager.Infrastructure.Database;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FoodManager.Application.Foods.Queries.GetFoodByIdQuery;

public class getFoodByIdHandler : IRequestHandler<GetFoodByIdQuery, GetFoodModel>
{
    private readonly DataBaseContext _context;
    private readonly IMapper _mapper;
    public getFoodByIdHandler(DataBaseContext context,
    IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<GetFoodModel> Handle(GetFoodByIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var getFoodById = await _context
               .Foods
               .Where(x => !x.IsDeleted)
               .FirstOrDefaultAsync(x => x.Id == request.Id);

            if (getFoodById is null)
            {
                throw new HttpResponseException
                {
                    Status = 404,
                    Value = new
                    {
                        Code = CodeErrorEnum.NOT_FOUND_RESOURCE.ToString(),
                        Message = "Comida não encontrada ou não existe",
                    }
                };
            }

            return _mapper.Map<GetFoodModel>(getFoodById);
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
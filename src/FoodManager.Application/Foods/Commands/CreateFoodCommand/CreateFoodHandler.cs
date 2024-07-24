using AutoMapper;
using FoodManager.Domain.Models;
using MediatR;

namespace FoodManager.Application.Foods.Commands.CreateFoodCommand;


public class CreateFoodHandler : IRequestHandler<CreateFoodCommand, bool>
{
    private readonly IMapper _mapper;
    public CreateFoodHandler(IMapper mapper)
    {
        _mapper = mapper;
    }

    public async Task<bool> Handle(CreateFoodCommand request, CancellationToken cancellationToken)
    {
        try
        {
            Food food = _mapper.Map<Food>(request);
            return true;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

}

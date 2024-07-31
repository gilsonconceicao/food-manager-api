using FoodManager.API.Enums;
using FoodManager.Application.Common.Exceptions;
using FoodManager.Infrastructure.Database;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FoodManager.Application.Foods.Commands.FoodUpdateCommand;

public class FoodUpdateHandler : IRequestHandler<FoodUpdateCommand, bool>
{
    private readonly DataBaseContext _context;
    public FoodUpdateHandler(DataBaseContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(FoodUpdateCommand request, CancellationToken cancellationToken)
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
                    Status = 400,
                    Value = new
                    {
                        Code = CodeErrorEnum.NOT_FOUND_RESOURCE.ToString(),
                        Message = "Comida não encontrada ou não existe",
                    }
                };
            }

    
            getFoodById.Name = request.Name;
            getFoodById.Price = request.Price;
            getFoodById.Category = request.Category;
            getFoodById.Description = request.Description;
            getFoodById.UrlImage = request.UrlImage;
            getFoodById.PreparationTime = request.PreparationTime;

            _context.Foods.Update(getFoodById);
            _context.Entry(getFoodById).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _context.SaveChanges();

            return true;
        }
        catch (HttpResponseException ex)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new HttpResponseException
            {
                Value = new
                {
                    Error = ex.Message,
                }
            };
        }
    }
}
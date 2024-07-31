using FoodManager.API.Enums;
using FoodManager.Application.Common.Exceptions;
using FoodManager.Infrastructure.Database;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FoodManager.Application.Foods.Commands.FoodDeleteCommand;

public class FoodDeleteHandler : IRequestHandler<FoodDeleteCommand, bool>
{
    private readonly DataBaseContext _context;

    public FoodDeleteHandler(DataBaseContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(FoodDeleteCommand request, CancellationToken cancellationToken)
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

            getFoodById.IsDeleted = true;
            await _context.SaveChangesAsync();

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
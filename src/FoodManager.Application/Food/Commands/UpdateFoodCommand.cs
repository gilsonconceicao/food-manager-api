using FoodManager.API.Enums;
using FoodManager.Application.Common.Exceptions;
using FoodManager.Domain.Enums;
using FoodManager.Infrastructure.Database;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FoodManager.Application.Foods.Commands;


public class FoodUpdateCommand : IRequest<bool>
{
    public FoodUpdateCommand(
        Guid Id,
        string Name,
        string UrlImage,
        string Description,
        bool IsAvailable,
        decimal Price,
        FoodCategoryEnum Category,
        string PreparationTime)
    {
        this.Id = Id;
        this.Name = Name;
        this.UrlImage = UrlImage;
        this.Description = Description;
        this.IsAvailable = IsAvailable;
        this.Price = Price;
        this.Category = Category;
        this.PreparationTime = PreparationTime;
    }
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string UrlImage { get; set; }
    public string Description { get; set; }
    public bool IsAvailable { get; set; }
    public decimal Price { get; set; }
    public FoodCategoryEnum Category { get; set; }
    public string PreparationTime { get; set; }
}

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
                .FirstOrDefaultAsync(x => x.Id == request.Id)
                ?? throw new HttpResponseException
                {
                    Status = 404,
                    Value = new
                    {
                        Code = CodeErrorEnum.NOT_FOUND_RESOURCE.ToString(),
                        Message = "Comida não encontrada ou não existe",
                    }
                };

    
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
        catch (HttpResponseException)
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
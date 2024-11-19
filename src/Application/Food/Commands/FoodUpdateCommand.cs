using Application.Common.Exceptions;
using Domain.Enums;
using Infrastructure.Database;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Foods.Commands;

public class FoodUpdateCommand : IRequest<bool>
{
    public FoodUpdateCommand(
        Guid Id,
        string Name,
        string UrlImage,
        string Description,
        bool IsAvailable,
        decimal Price,
        FoodCategoryEnum Category)
    {
        this.Id = Id;
        this.Name = Name;
        this.UrlImage = UrlImage;
        this.Description = Description;
        this.IsAvailable = IsAvailable;
        this.Price = Price;
        this.Category = Category;
    }
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public string? UrlImage { get; set; }
    public string? Description { get; set; }
    public bool? IsAvailable { get; set; }
    public decimal? Price { get; set; }
    public FoodCategoryEnum? Category { get; set; }
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
        var getFoodById = await _context
            .Foods
            .Where(x => !x.IsDeleted)
            .FirstOrDefaultAsync(x => x.Id == request.Id)
            ?? throw new NotFoundException("Comida não encontrada ou não existe.");


        if (request.Name != null)
            getFoodById.Name = request.Name;

        if (request.Price != null)
            getFoodById.Price = (decimal)request.Price;

        if (request.Category != null)
            getFoodById.Category = request.Category;

        if (request.Description != null)
            getFoodById.Description = request.Description;

        if (request.UrlImage != null)
            getFoodById.UrlImage = request.UrlImage;


        _context.Foods.Update(getFoodById);
        _context.Entry(getFoodById).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
        await _context.SaveChangesAsync();

        return true;
    }
}

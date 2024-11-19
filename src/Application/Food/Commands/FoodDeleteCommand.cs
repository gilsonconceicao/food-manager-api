using Application.Common.Exceptions;
using Infrastructure.Database;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Foods.Commands;

public class FoodDeleteCommand : IRequest<bool>
{
    public Guid Id { get; set; }
    public FoodDeleteCommand(Guid id)
    {
        this.Id = id;
    }
}

public class FoodDeleteHandler : IRequestHandler<FoodDeleteCommand, bool>
{
    private readonly DataBaseContext _context;

    public FoodDeleteHandler(DataBaseContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(FoodDeleteCommand request, CancellationToken cancellationToken)
    {
        var getFoodById = await _context
            .Foods
            .Where(x => !x.IsDeleted)
            .FirstOrDefaultAsync(x => x.Id == request.Id)
            ?? throw new NotFoundException("Comida não encontrada ou não existe.");

        getFoodById.IsDeleted = true;
        await _context.SaveChangesAsync();
        return true;
    }
}

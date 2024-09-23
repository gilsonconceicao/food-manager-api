using FoodManager.API.Enums;
using FoodManager.Application.Common.Exceptions;
using FoodManager.Domain.Enums;
using FoodManager.Domain.Models;
using FoodManager.Domain.StateManagement;
using FoodManager.Infrastructure.Database;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FoodManager.Application.Orders.Commands;

public class UpdateStepOrderCommand : IRequest<OrderStatus>
{
    public Guid Id { get; set; }
    public OrderStatus NewStatus { get; set; }

}

public class UpdateStepOrderHandler : IRequestHandler<UpdateStepOrderCommand, OrderStatus>
{
    private readonly DataBaseContext _context;
    public UpdateStepOrderHandler(DataBaseContext database)
    {
        _context = database;
    }
    public async Task<OrderStatus> Handle(UpdateStepOrderCommand request, CancellationToken cancellationToken)
    {
        try
        {
            Order order = await _context.Orders
                .Where(x => !x.IsDeleted)
                .FirstOrDefaultAsync(x => x.Id == request.Id)
                ?? throw new HttpResponseException
                {
                    Status = 404,
                    Value = new
                    {
                        Code = CodeErrorEnum.NOT_FOUND_RESOURCE.ToString(),
                        Message = "Pedido não encontrada ou não existe",
                    }
                };

           var newStatusStateless = new OrderStateless(order); 

            newStatusStateless.ProcessAsync();  
            await _context.SaveChangesAsync();
            return order.Status;
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
using FoodManager.API.Enums;
using FoodManager.Application.Common.Exceptions;
using FoodManager.Domain.Enums;
using FoodManager.Domain.Enums.Triggers;
using FoodManager.Domain.Models;
using FoodManager.Domain.StateManagement;
using FoodManager.Infrastructure.Database;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FoodManager.Application.Orders.Commands;

public class ExecuteTriggerCommand : IRequest<OrderStatus>
{
    public Guid Id { get; set; }
    public OrderTrigger Trigger { get; set; }

}

public class ExecuteTriggerCommandHandler : IRequestHandler<ExecuteTriggerCommand, OrderStatus>
{
    private readonly DataBaseContext _context;
    public ExecuteTriggerCommandHandler(DataBaseContext database)
    {
        _context = database;
    }
    public async Task<OrderStatus> Handle(ExecuteTriggerCommand request, CancellationToken cancellationToken)
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

            var stateless = new OrderStateless(order);

            switch (request.Trigger)
            {
                case OrderTrigger.ConfirmOrder:
                    await stateless.ConfirmOrderAsync();
                    break;
                case OrderTrigger.CheckHowDone:
                    await stateless.CheckHowDoneAsync();
                    break;
                case OrderTrigger.Finish:
                    await stateless.FinishedAsync();
                    break;
                case OrderTrigger.Cancel:
                    await stateless.CancelAsync();
                    break;
                default:
                    await stateless.ProcessAsync();
                    break;
            }

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
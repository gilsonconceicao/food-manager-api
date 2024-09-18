using FoodManager.Domain.Enums;
using FoodManager.Domain.Enums.Triggers;
using FoodManager.Domain.Models;
using Stateless;

namespace FoodManager.Application.Stateless;

public class OrderStateless
{
    private StateMachine<OrderStatus, OrderTrigger> _machine;

    public OrderStateless(Order order)
    {
        _machine = new StateMachine<OrderStatus, OrderTrigger>(order.Status);

        // Configuração dos estados e transições
        _machine.Configure(OrderStatus.Requested)
            .Permit(OrderTrigger.Process, OrderStatus.AwaitingConfirmation)
            .Permit(OrderTrigger.Cancel, OrderStatus.Canceled);

        _machine.Configure(OrderStatus.AwaitingConfirmation)
            .Permit(OrderTrigger.ConfirmOrder, OrderStatus.InPreparation)
            .PermitIf(OrderTrigger.Cancel, OrderStatus.Canceled);

        _machine.Configure(OrderStatus.InPreparation)
            .Permit(OrderTrigger.CheckHowDone, OrderStatus.Done);

        _machine.Configure(OrderStatus.Done).Permit(OrderTrigger.Finish, OrderStatus.Finished); 
    }

    public async void ProcessAsync() => await _machine.FireAsync(OrderTrigger.Process);
    public async void ConfirmOrderAsync() => await _machine.FireAsync(OrderTrigger.ConfirmOrder);
    public async void CancelAsync() => await _machine.FireAsync(OrderTrigger.Cancel);
    public async void CheckHowDone() => await _machine.FireAsync(OrderTrigger.CheckHowDone);
    public async void Finished() => await _machine.FireAsync(OrderTrigger.Finish);
}
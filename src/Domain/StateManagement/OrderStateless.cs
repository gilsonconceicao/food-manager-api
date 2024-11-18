using Domain.Enums;
using Domain.Enums.Triggers;
using Domain.Models;
using Stateless;

namespace Domain.StateManagement
{
    public class OrderStateless
    {
        private StateMachine<OrderStatus, OrderTrigger> _machine;
        private Order _order;

        public OrderStateless(Order order)
        {
            _order = order;
            _machine = new StateMachine<OrderStatus, OrderTrigger>(order.Status);

            // Configuring states and triggers
            _machine.Configure(OrderStatus.Created)
                .Permit(OrderTrigger.Process, OrderStatus.Requested);

            _machine.Configure(OrderStatus.Requested)
                .Permit(OrderTrigger.Process, OrderStatus.AwaitingConfirmation)
                .Permit(OrderTrigger.Cancel, OrderStatus.Canceled);

            _machine.Configure(OrderStatus.AwaitingConfirmation)
                .Permit(OrderTrigger.ConfirmOrder, OrderStatus.InPreparation)
                .PermitIf(OrderTrigger.Cancel, OrderStatus.Canceled);

            _machine.Configure(OrderStatus.InPreparation)
                .Permit(OrderTrigger.CheckHowDone, OrderStatus.Done);

            _machine.Configure(OrderStatus.Done)
                .Permit(OrderTrigger.Finish, OrderStatus.Finished);
        }

        public async Task ProcessAsync() => await FireTriggerAsync(OrderTrigger.Process);
        public async Task ConfirmOrderAsync() => await FireTriggerAsync(OrderTrigger.ConfirmOrder);
        public async Task CancelAsync() => await FireTriggerAsync(OrderTrigger.Cancel);
        public async Task CheckHowDoneAsync() => await FireTriggerAsync(OrderTrigger.CheckHowDone);
        public async Task FinishedAsync() => await FireTriggerAsync(OrderTrigger.Finish);

        private async Task FireTriggerAsync(OrderTrigger trigger)
        {
            if (_machine.CanFire(trigger))
            {
                _machine.Fire(trigger);
                _order.Status = _machine.State;
                await Task.CompletedTask;
            }
            else
            {
                throw new InvalidOperationException($"Cannot process trigger: {trigger} in state: {_machine.State}");
            }
        }
    }
}

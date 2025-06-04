using Domain.Enums;
using Domain.Common.Exceptions;
using Infrastructure.Database;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;

namespace Application.Orders.Commands
{
    public class OrderCancelCommand : IRequest<bool>
    {
        public Guid OrderId { get; set; }
    }

    public class OrderCancelHandler : IRequestHandler<OrderCancelCommand, bool>
    {
        private readonly DataBaseContext _context;

        public OrderCancelHandler(DataBaseContext dataBaseContext)
        {
            _context = dataBaseContext;
        }

        public async Task<bool> Handle(OrderCancelCommand request, CancellationToken cancellationToken)
        {
            var order = await _context.Orders
                .Where(o => !o.IsDeleted)
                .FirstOrDefaultAsync(x => x.Id == request.OrderId)
                ?? throw new NotFoundException("Pedido não encontrado ou não existe.");

            var statusAvailableToDelete = new List<OrderStatus>
            {
                OrderStatus.AwaitingPayment,
                OrderStatus.PaymentFailed,
                OrderStatus.Expired
            };

            if (!statusAvailableToDelete.Contains(order.Status))
            {
                throw new HttpResponseException(
                    StatusCodes.Status400BadRequest,
                    CodeErrorEnum.INVALID_BUSINESS_RULE.ToString(),
                    "Só é permitido exluir pedido nos status 'Aguardando pagamento', 'falha no pagamento' ou 'expirado'."
                );
            }

            order.Status = OrderStatus.Cancelled;
            await _context.SaveChangesAsync();
            return true;
        }

    }
}
using Domain.Enums;
using Domain.Common.Exceptions;
using Infrastructure.Database;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Application.Utils;

namespace Application.Orders.Commands
{
    public class OrderDeleteCommand : IRequest<bool>
    {
        public Guid OrderId { get; set; }
        public bool IsPermanent { get; set; }
    }
    public class OrderDeleteHandler : IRequestHandler<OrderDeleteCommand, bool>
    {
        private readonly DataBaseContext _context;

        public OrderDeleteHandler(DataBaseContext dataBaseContext)
        {
            _context = dataBaseContext;
        }

        public async Task<bool> Handle(OrderDeleteCommand request, CancellationToken cancellationToken)
        {
            var order = await _context.Orders
                .Where(o => !o.IsDeleted)
                .FirstOrDefaultAsync(x => x.Id == request.OrderId)
                ?? throw new NotFoundException("Pedido não encontrado ou não existe.");

            var statusAvailableToDelete = new List<OrderStatus>
            {
                OrderStatus.Cancelled,
                OrderStatus.PaymentFailed
            };

            if (!statusAvailableToDelete.Contains(order.Status))
            {
                throw new HttpResponseException(
                StatusCodes.Status400BadRequest,
                CodeErrorEnum.INVALID_BUSINESS_RULE.ToString(),
                "Só é permitido exluir pedido nos status 'Aguardando pagamento', 'Cancelado' ou 'expirado'."
            );
            }

            order.IsDeleted = true;
            if (!!request.IsPermanent)
            {
                _context.Remove(order);
            }
            await _context.SaveChangesAsync();
            return true;
        }

    }
}
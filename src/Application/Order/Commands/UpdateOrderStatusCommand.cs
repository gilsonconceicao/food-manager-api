using Domain.Enums;
using Domain.Common.Exceptions;
using Infrastructure.Database;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Api.Services;

namespace Application.Orders.Commands
{
    public class UpdateOrderStatusCommand : IRequest<bool>
    {
        public Guid OrderId { get; set; }
    }
    public class UpdateOrderStatusHandler : IRequestHandler<UpdateOrderStatusCommand, bool>
    {
        private readonly DataBaseContext _context;
        private readonly ICurrentUser _httpUserService;


        public UpdateOrderStatusHandler(
            DataBaseContext dataBaseContext,
            ICurrentUser currentUser
        )
        {
            _context = dataBaseContext;
            _httpUserService = currentUser;
        }

        public async Task<bool> Handle(UpdateOrderStatusCommand request, CancellationToken cancellationToken)
        {
            var userAuthenticated = await _httpUserService.GetAuthenticatedUser();
            var userId = userAuthenticated.UserId;

            var user = await _context
                .Users
                .FirstOrDefaultAsync(u => u.FirebaseUserId == userId)
                ?? throw new NotFoundException("usuário não encontrado ou não existe.");

            var order = await _context.Orders
                .Where(o => !o.IsDeleted)
                .FirstOrDefaultAsync(x => x.Id == request.OrderId)
                ?? throw new NotFoundException("Pedido não encontrado ou não existe.");


            if (!user.IsRoot)
            {
                throw new HttpResponseException(
                    StatusCodes.Status400BadRequest,
                    CodeErrorEnum.INVALID_BUSINESS_RULE.ToString(),
                    "Não foi possível seguir, ação exclusiva para administrador do sistema"
                );
            }

            var newStatus = order.Status switch
            {
                OrderStatus.Paid => OrderStatus.InPreparation,
                OrderStatus.InPreparation => OrderStatus.Done,
                OrderStatus.Done => OrderStatus.Delivery,
                OrderStatus.Delivery => OrderStatus.Finished,
                _ =>
                throw new HttpResponseException(
                    StatusCodes.Status400BadRequest,
                    CodeErrorEnum.INVALID_BUSINESS_RULE.ToString(),
                    "Status atual do pedido não pode ser processado para o próximo."
                )
            };

            order.Status = newStatus;

            await _context.SaveChangesAsync();
            return true;
        }

    }
}
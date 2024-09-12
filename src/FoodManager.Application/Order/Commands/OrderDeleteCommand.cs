using FoodManager.API.Enums;
using FoodManager.Application.Common.Exceptions;
using FoodManager.Infrastructure.Database;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FoodManager.Application.Orders.Commands
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
            try
            {
                var order = await _context.Orders
                    .Where(o => !o.IsDeleted)
                    .FirstOrDefaultAsync(x => x.Id == request.OrderId)
                    ?? throw new HttpResponseException
                    {
                        Status = 404,
                        Value = new
                        {
                            Code = CodeErrorEnum.NOT_FOUND_RESOURCE.ToString(),
                            Message = "Pedido não encontrada ou não existe",
                        }
                    };


                order.IsDeleted = true;
                if (!!request.IsPermanent) 
                {
                    _context.Remove(order); 
                }
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new HttpResponseException
                {
                    Status = 500,
                    Value = new
                    {
                        Message = ex.Message,
                    }
                };
            }
        }
    }
}
using FluentValidation;

namespace FoodManager.Application.FoodsOrders.Commands.OrderCreateCommand;

public class OrderCreateValidations : AbstractValidator<OrderCreateCommand>
{
    public OrderCreateValidations()
    {
        RuleFor(c => c.Status).NotNull().NotEmpty().WithMessage($"Status é obrigatório");
        RuleFor(c => c.Client).NotNull().NotEmpty().WithMessage($"Cliente é obrigatório");
    }
}
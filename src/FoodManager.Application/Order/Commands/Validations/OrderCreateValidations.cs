using FluentValidation;

namespace FoodManager.Application.Orders.Commands.Validatons;

public class OrderCreateValidations : AbstractValidator<OrderCreateCommand>
{
    public OrderCreateValidations()
    {
        // RuleFor(c => c.Client).NotNull().NotEmpty().WithMessage($"Cliente é obrigatório");
    }
}
using FluentValidation;

namespace Application.Orders.Commands.Validatons;

public class OrderCreateValidations : AbstractValidator<OrderCreateCommand>
{
    public OrderCreateValidations()
    {
        RuleFor(c => c.CartIds)
            .Must(x => x == null || x.Any())
            .WithMessage("É necessário informar ao menos um item");
    }
}
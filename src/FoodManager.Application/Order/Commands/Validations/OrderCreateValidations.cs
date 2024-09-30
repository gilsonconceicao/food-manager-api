using FluentValidation;

namespace FoodManager.Application.Orders.Commands.Validatons;

public class OrderCreateValidations : AbstractValidator<OrderCreateCommand>
{
    public OrderCreateValidations()
    {
        RuleFor(c => c.Foods)
            .Must(x => x == null || x.Any())
            .WithMessage("É necessário informar ao menos uma comida");
    }
}
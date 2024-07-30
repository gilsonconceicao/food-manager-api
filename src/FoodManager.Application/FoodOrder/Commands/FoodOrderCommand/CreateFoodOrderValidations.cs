using FluentValidation;

namespace FoodManager.Application.FoodOrders.Commands.CreateFoodOrderCommand;

public class CreateFoodOrderValidations : AbstractValidator<CreateFoodOrderCommand>
{
    public CreateFoodOrderValidations()
    {
        // RuleFor(c => c.Status).NotNull().NotEmpty().WithMessage($"Status é obrigatório");
        // RuleFor(c => c.Client).NotNull().NotEmpty().WithMessage($"Cliente é obrigatório");
        // RuleFor(c => c.RequestNumber)
        //     .NotEmpty()
        //     .WithMessage($"Número do pedido é obrigatório")
        //     .GreaterThan(0)
        //     .WithMessage($"Número do pedido precisa ser maior ou igual a 0");;
    }
}
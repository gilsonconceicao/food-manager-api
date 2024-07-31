using FluentValidation;

namespace FoodManager.Application.Foods.Commands.FoodCreateCommand
{
    public class FoodCreateValidations : AbstractValidator<FoodCreateCommand>
    {
        public FoodCreateValidations()
        {
            RuleFor(c => c.Price)
                .NotNull()
                .NotEmpty()
                .WithMessage($"Preço é obrigatório")
                .GreaterThan(3)
                .WithMessage($"Preço precisa ser maior ou igual a 3");

            RuleFor(c => c.Category).NotNull().NotEmpty().WithMessage($"Categoria é obrigatório");
            RuleFor(c => c.Name).NotNull().NotEmpty().WithMessage($"Nome é obrigatório");
            RuleFor(c => c.Description).NotEmpty().WithMessage($"Descrição é obrigatório");
        }
    }
}
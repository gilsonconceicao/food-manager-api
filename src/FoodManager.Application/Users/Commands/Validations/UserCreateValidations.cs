using FluentValidation;

namespace FoodManager.Application.Users.Commands.Validations; 

public class UserCreateValidation : AbstractValidator<UserCreateCommand>
{
    public UserCreateValidation()
    {
        RuleFor(x => x.RegistrationNumber)
           .NotNull()
           .NotEmpty()
           .WithMessage("CPF é obrigatório");

        RuleFor(x => x.Name)
           .NotNull()
           .NotEmpty()
           .WithMessage($"Nome é obrigatório"); 
    }
}
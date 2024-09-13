using System.Data;
using FluentValidation;
using FoodManager.Application.Utils;

namespace FoodManager.Application.Users.Commands.Validations;

public class UserCreateValidation : AbstractValidator<UserCreateCommand>
{
    public UserCreateValidation()
    {
        RuleFor(x => x.RegistrationNumber)
           .NotNull()
           .NotEmpty()
           .WithMessage("CPF é obrigatório")
           .Must(x => ValidationsUtils.IsValidRegistrationNumber(x))
           .WithMessage("CPF informado é inválido");

        RuleFor(x => x.Name)
           .NotNull()
           .NotEmpty()
           .WithMessage($"Nome é obrigatório");
    }
}
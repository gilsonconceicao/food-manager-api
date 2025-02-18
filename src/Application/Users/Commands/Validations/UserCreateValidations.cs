using System.Data;
using FluentValidation;
using Application.Utils;

namespace Application.Users.Commands.Validations;

public class UserCreateValidation : AbstractValidator<UserCreateCommand>
{
    public UserCreateValidation()
    {
        RuleFor(x => x.PhoneNumber)
           .NotNull()
           .NotEmpty()
           .WithMessage("Número de telefone é obrigatório"); 
        RuleFor(x => x.Name)
           .NotNull()
           .NotEmpty()
           .WithMessage($"Nome é obrigatório");
    }
}
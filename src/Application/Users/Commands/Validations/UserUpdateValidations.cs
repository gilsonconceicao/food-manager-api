using System.Data;
using FluentValidation;
using Application.Utils;

namespace Application.Users.Commands.Validations; 

public class UserUpdateValidations : AbstractValidator<UserUpdateCommand>
{
    public UserUpdateValidations()
    {
        RuleFor(x => x.Name)
           .NotNull()
           .NotEmpty()
           .WithMessage($"Nome é obrigatório"); 
    }
}
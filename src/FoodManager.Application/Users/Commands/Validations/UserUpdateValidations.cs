using System.Data;
using FluentValidation;
using FoodManager.Application.Utils;

namespace FoodManager.Application.Users.Commands.Validations; 

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
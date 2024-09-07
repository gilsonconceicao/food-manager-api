using System.ComponentModel.DataAnnotations;
using FoodManager.API.Enums;
using FoodManager.Application.Common.Exceptions;

namespace FoodManager.Application.Utils;
#nullable disable
static class ErrorUtils
{
    public static List<FailureError> ValidationFailure(List<FluentValidation.Results.ValidationFailure> errors)
    {
        var mapperProperties = errors.Select(x => new FailureError
        {
            Message = x.ErrorMessage,
            Field = x.PropertyName
        }).ToList();

        return mapperProperties;
    }

    
    public static object InvalidFieldsError(FluentValidation.Results.ValidationResult validations, string Message = "Erro ao validar campos")
    {
        throw new HttpResponseException
        {
            Status = 400,
            Value = new
            {
                Code = CodeErrorEnum.INVALID_FORM_FIELDS.ToString(),
                Message = "Erro ao validar campos",
                Details = ErrorUtils.ValidationFailure(validations.Errors)
            }
        };
    }
}

class FailureError
{
    public string Message { get; set; }
    public string Field { get; set; }
}
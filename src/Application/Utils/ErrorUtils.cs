using System.ComponentModel.DataAnnotations;
using Api.Enums;
using Application.Common.Exceptions;
using Microsoft.AspNetCore.Http;

namespace Application.Utils;
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

    
    public static object InvalidFieldsError(FluentValidation.Results.ValidationResult validations)
    {
        var errorsList = validations.Errors;
        throw new HttpResponseException
        {
            Status = 400,
            Value = new
            {
                Code = CodeErrorEnum.INVALID_FORM_FIELDS.ToString(),
                Message = errorsList.Count > 1 ? "Erro ao validar os campos" : $"Campo {validations.Errors[0].PropertyName} inválido",
                Details = ErrorUtils.ValidationFailure(validations.Errors)
            }
        };
    }

    public static object NotFoudException(string? Message)
    {
        throw new HttpResponseException
        {
            Status = StatusCodes.Status404NotFound,
            Value = new
            {
                Code = CodeErrorEnum.INVALID_FORM_FIELDS.ToString(),
                Message = Message ?? "Recuro nõao encontrado",
            }
        };
    }
}

class FailureError
{
    public string Message { get; set; }
    public string Field { get; set; }
}
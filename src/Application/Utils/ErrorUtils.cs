using System.ComponentModel.DataAnnotations;
using Domain.Enums;
using Domain.Common.Exceptions;
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
          throw new HttpResponseException(
            StatusCodes.Status400BadRequest, 
            CodeErrorEnum.INVALID_FORM_FIELDS.ToString(), 
            errorsList.Count > 1 ? "Erro ao validar os campos" : $"Campo {validations.Errors[0].PropertyName} inválido", 
            new
            {
                Details = ErrorUtils.ValidationFailure(validations.Errors)
            }
        );
    }

    public static object NotFoudException(string? Message)
    {
        throw new HttpResponseException(
            StatusCodes.Status404NotFound, 
            CodeErrorEnum.NOT_FOUND_RESOURCE.ToString(), 
            Message ?? "Recuro não encontrado"
        );
    }

    public static object BadRequestException(string? Message)
    {
        throw new HttpResponseException(
            StatusCodes.Status400BadRequest, 
            CodeErrorEnum.INVALID_BUSINESS_RULE.ToString(), 
            Message ?? "Erro desconhecido"
        );
    }
}

class FailureError
{
    public string Message { get; set; }
    public string Field { get; set; }
}
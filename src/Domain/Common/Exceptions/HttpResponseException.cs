using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

#nullable disable
namespace Domain.Common.Exceptions;


public class HttpResponseException : Exception
{
    public int StatusCode { get; }
    public string Code { get; }
    public object Value { get; }

    public HttpResponseException(int statusCode, string code, string message, object value = null) : base(message)
    {
        StatusCode = statusCode;
        Code = code;
        Value = value;
    }
}

public class NotFoundException : HttpResponseException
{
    public NotFoundException(string message)
        : base(StatusCodes.Status404NotFound, "NOT_FOUND", message) { }
}

public class UnauthorizedException : HttpResponseException
{
    public UnauthorizedException(string message = "Usuário não autorizado.")
        : base(StatusCodes.Status401Unauthorized, "UNAUTHORIZED", message) { }
}

public class HttpResponseExceptionFilter : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        (int status, string code, string message, object details) = context.Exception switch
        {
            HttpResponseException httpEx => (
                httpEx.StatusCode,
                httpEx.Code,
                httpEx.Message,
                httpEx.Value ?? httpEx.Message
            ),

            UnauthorizedAccessException unauthorized => (
                StatusCodes.Status401Unauthorized,
                "UNAUTHORIZED",
                "Usuário não autorizado.",
                null
            ),

            _ => (
                StatusCodes.Status500InternalServerError,
                "INTERNAL_SERVER_ERROR",
                "Ocorreu um erro inesperado no servidor.",
                context.Exception.Message
            )
        };

        context.Result = new ObjectResult(new
        {
            code,
            message,
            details
        })
        {
            StatusCode = status
        };

        context.ExceptionHandled = true;
    }
}
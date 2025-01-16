using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

#nullable disable
namespace Application.Common.Exceptions;

public class HttpResponseException : Exception
{
    public int Status { get; set; } = 500;
    public object Value { get; set; }
}

public class NotFoundException : Exception
{
    public NotFoundException(string message) : base(message) { }
}

public class HttpResponseExceptionFilter : ExceptionFilterAttribute
{
    public override void OnException(ExceptionContext context)
    {
        if (context.Exception is HttpResponseException httpResponseException)
        {
            context.Result = new ObjectResult(httpResponseException.Value)
            {
                StatusCode = httpResponseException.Status,
            };
        }
        else if (context.Exception is NotFoundException notFoundException)
        {
            context.Result = new ObjectResult(new
            {
                Message = notFoundException.Message
            })
            {
                StatusCode = StatusCodes.Status404NotFound,
            };
        }
        else if (context.Exception is UnauthorizedAccessException unauthorizedAccessException)
        {
            context.Result = new UnauthorizedObjectResult(new
            {
                Message = "Usuário não autorizado a realizar esta solicitação."
            })
            {
                StatusCode = StatusCodes.Status401Unauthorized, 
            };
        }
        else
        {
            context.Result = new ObjectResult(new
            {
                Message = "An unexpected error occurred.",
                Details = context.Exception.Message,
                StackTrace = context.Exception.StackTrace
            })
            {
                StatusCode = StatusCodes.Status500InternalServerError,
            };
        }

    }
}

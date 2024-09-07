using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

#nullable disable
namespace FoodManager.Application.Common.Exceptions;

public class HttpResponseException : Exception
{
    public int Status { get; set; } = 500;
    public object Value { get; set; }
}

public class HttpResponseExceptionFilter : IActionFilter, IOrderedFilter
{
    public int Order { get; } = int.MaxValue - 10;
    public void OnActionExecuting(ActionExecutingContext context) { }
    public void OnActionExecuted(ActionExecutedContext context)
    {
        if (context.Exception != null)
        {
            if (context.Exception is HttpResponseException exception)
            {
                context.Result = new ObjectResult(exception.Value)
                {
                    StatusCode = exception.Status,
                };
            }
            else
            {
                context.Result = new ObjectResult(new
                {
                    Message = "An unexpected error occurred.",
                    Details = context.Exception.Message
                })
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                };
            }

            context.ExceptionHandled = true;
        }
    }
}
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
}

class FailureError
{
    public string Message { get; set;}
    public string Field { get; set;}
}
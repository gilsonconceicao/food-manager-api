namespace FoodManager.Domain.Extensions; 
#nullable disable
public class ListDataResponse<T>
{
    public double? Count { get; set; }
    public T Data { get; set; }
}
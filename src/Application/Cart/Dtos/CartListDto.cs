using Domain.Models;
namespace Application.Carts.Dtos; 
#nullable disable

public class CartListDto 
{
    public List<CartDto> Data { get; set;}
    public SummaryCartDto Summary { get; set;}
}
using crud_products_api.src.Models;

namespace FoodManager.Domain.Models;
#nullable disable
public class Client : BaseEntity
{
    public string Name { get; set; }
    public string DocumentNumber { get; set; }
    public string PhoneNumber { get; set; }
    public Address Address { get; set; } 
    public Guid AddressId { get; set; } 
    public Guid OrderId { get; set; }
    public Order Order { get; set; }
}

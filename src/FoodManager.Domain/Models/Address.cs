using FoodManager.Domain.Models;

namespace crud_products_api.src.Models;

#nullable disable
public class Address
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid ClientId { get; set; } 
    public Client Client { get; set; } 
    public string Street { get; set; }
    public string City { get; set; }
    public string State { get; set; }
    public string ZipCode { get; set; }
}
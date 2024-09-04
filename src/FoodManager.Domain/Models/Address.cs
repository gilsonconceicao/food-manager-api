using FoodManager.Domain.Models;

namespace FoodManager.Domain.Models;

#nullable disable
public class Address
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Street { get; set; }
    public string City { get; set; }
    public string State { get; set; }
    public string ZipCode { get; set; }
    public User? User { get; set; }
    public Guid? UserId { get; set; }
}
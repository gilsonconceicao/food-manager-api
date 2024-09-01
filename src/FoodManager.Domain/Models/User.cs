namespace FoodManager.Domain.Models; 
#nullable disable
public class User : BaseEntity
{
    public string Name { get; set; }
    public string RegistrationNumber { get; set; }
    public Address Address { get; set; }
}
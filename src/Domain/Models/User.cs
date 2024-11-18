namespace Domain.Models; 
#nullable disable
public class User : BaseEntity
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string FirebaseUserId { get; set; }
    public string RegistrationNumber { get; set; }
    public Address Address { get; set; }
    public ICollection<Order> Orders { get; set; } 
}
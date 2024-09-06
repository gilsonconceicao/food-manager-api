namespace FoodManager.Application.Users.Dtos;
#nullable disable 
public class AddressGetDto
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Street { get; set; }
    public string City { get; set; }
    public string State { get; set; }
    public string ZipCode { get; set; }
    public Guid? UserId { get; set; }
}
namespace Application.Users.Dtos;
#nullable disable 
public class AddressCreateDto
{
    public string Street { get; set; }
    public string City { get; set; }
    public string State { get; set; }
    public int Number { get; set; }
    public string ZipCode { get; set; }
    public string Complement { get; set; }

}
namespace Application.Users.Dtos;
#nullable disable 
public class AddressGetDto
{
    public string Street { get; set; }
    public int Number { get; set; }
    public string City { get; set; }
    public string State { get; set; }
    public string ZipCode { get; set; }
    public string Complement { get; set; }

}
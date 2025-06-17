using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Models;
#nullable disable
public class User : BaseEntity
{
    [Column(TypeName = "varchar(60)")]
    public string Name { get; set; }

    [Column(TypeName = "varchar(120)")]
    public string Email { get; set; }

    [Column(TypeName = "varchar(35)")]
    public string FirebaseUserId { get; set; }

    [Column(TypeName = "varchar(11)")]
    public string PhoneNumber { get; set; }

    public ICollection<Order> Orders { get; set; }

    public bool IsRoot { get; set; }

    public Guid? AddressId { get; set; }
    public Address? Address { get; set; }
}

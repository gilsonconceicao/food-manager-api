using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Models;
#nullable disable
namespace Domain.Models;

#nullable disable
public class Address
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid? UserId { get; set; }
    public User? User { get; set; }

    [Column(TypeName = "varchar(100)")]
    public string Street { get; set; }

    [Column(TypeName = "varchar(25)")]
    public string City { get; set; }

    [Column(TypeName = "varchar(2)")]
    public string State { get; set; }

    [Column(TypeName = "varchar(8)")]
    public string ZipCode { get; set; }

    [Column(TypeName = "int")]
    public int Number { get; set; }
}
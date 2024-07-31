
using crud_products_api.src.Models;
using FoodManager.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class AddressConfiguration : IEntityTypeConfiguration<Address>
{
    public void Configure(EntityTypeBuilder<Address> builder)
    {
        builder
            .HasOne(x => x.Client)
            .WithOne(x => x.Address)
            .HasForeignKey<Client>(x => x.AddressId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
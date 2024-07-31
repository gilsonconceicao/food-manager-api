using crud_products_api.src.Models;
using FoodManager.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class ClientConfiguration : IEntityTypeConfiguration<Client>
{
    public void Configure(EntityTypeBuilder<Client> builder)
    {
        builder
            .HasOne(x => x.Address)
            .WithOne(x => x.Client)
            .HasForeignKey<Client>(x => x.AddressId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
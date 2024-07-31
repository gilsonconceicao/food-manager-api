using FoodManager.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder) 
    { 
        builder
            .HasOne(x => x.Client)
            .WithOne(x => x.Order)
            .HasForeignKey<Client>(x => x.OrderId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasMany(x => x.Foods)
            .WithOne(x => x.Order)
            .HasForeignKey(x => x.OrderId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
using FoodManager.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class FoodOrderConfiguration : IEntityTypeConfiguration<FoodOrder>
{
    public void Configure(EntityTypeBuilder<FoodOrder> builder) 
    { 
        builder
            .HasOne(x => x.Client)
            .WithOne(x => x.FoodOrder)
            .HasForeignKey<Client>(x => x.FoodOrderId);

        builder
            .HasMany(x => x.Foods)
            .WithOne(x => x.FoodOrder)
            .HasForeignKey(x => x.FoodOrderId);
    }
}
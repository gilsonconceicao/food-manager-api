using FoodManager.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.HasKey(x => x.Id);

        builder
            .HasMany(e => e.Foods)
            .WithMany(e => e.Orders)
            .UsingEntity<OrderFoodRelated>(
                x => x.HasOne<Food>().WithMany().HasForeignKey(e => e.FoodId),
                x => x.HasOne<Order>().WithMany().HasForeignKey(e => e.OrderId));
    }
}

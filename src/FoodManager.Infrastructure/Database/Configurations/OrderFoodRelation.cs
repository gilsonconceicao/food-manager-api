using FoodManager.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class OrderFoodRelation : IEntityTypeConfiguration<FoodOrderRelation>
{
    public void Configure(EntityTypeBuilder<FoodOrderRelation> builder)
    {
        builder
    .HasKey(fo => new { fo.FoodId, fo.OrderId });

        builder
            .HasOne(fo => fo.Order)
            .WithMany(o => o.FoodOrderRelations)
            .HasForeignKey(fo => fo.OrderId);

        builder
            .HasOne(fo => fo.Food)
            .WithMany(f => f.FoodOrderRelations)
            .HasForeignKey(fo => fo.FoodId);
    }
}

using FoodManager.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class OrdersFoodsRelationshipConfiguration : IEntityTypeConfiguration<OrdersFoodsRelationship>
{
    public void Configure(EntityTypeBuilder<OrdersFoodsRelationship> builder)
    {
        builder.HasKey(okr => new { okr.FoodId, okr.OrderId });

        builder.HasOne(okr => okr.Food)
               .WithMany(f => f.OrdersFoodsRelationship)
               .HasForeignKey(okr => okr.FoodId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(okr => okr.Order)
               .WithMany(o => o.OrdersFoodsRelationship)
               .HasForeignKey(okr => okr.OrderId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}

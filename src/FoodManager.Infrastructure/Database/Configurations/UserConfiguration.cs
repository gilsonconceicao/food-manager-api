using FoodManager.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        // builder.HasKey(x => x.Id);

        // builder
        //     .HasOne(x => x.Address)
        //     .WithOne(x => x.User)
        //     .HasForeignKey<Address>(x => x.UserId)
        //     .IsRequired(false);


        // builder
        //     .HasMany(x => x.Orders)
        //     .WithOne(x => x.User)
        //     .HasForeignKey(x => x.UserId)
        //     .IsRequired(false);
    }
}
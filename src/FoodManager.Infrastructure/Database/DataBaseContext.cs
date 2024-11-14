using System.Reflection;
using FoodManager.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace FoodManager.Infrastructure.Database
{
    public class DataBaseContext : DbContext
    {
        
        public DataBaseContext(DbContextOptions options) : base(options)
        { }

        public DbSet<Food> Foods { get; set; }
        public DbSet<OrderItems> Items { get; set; }
        public DbSet<Address> Address { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Cart> Carts { get; set; }

        public async Task MigrateAsync() => await base.Database.MigrateAsync();
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new OrderItemsConfiguration());
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(modelBuilder);
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                var entriesData = ChangeTracker.Entries<BaseEntity>();

                foreach (var entry in entriesData)
                {

                }


                var result = await base.SaveChangesAsync(cancellationToken);
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
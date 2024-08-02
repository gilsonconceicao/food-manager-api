using System.Reflection;
using crud_products_api.src.Models;
using FoodManager.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace FoodManager.Infrastructure.Database
{
    public class DataBaseContext : DbContext
    {
        public DataBaseContext(DbContextOptions options) : base(options)
        { }

        public DbSet<Food> Foods { get; set; }
        public DbSet<Address> Address { get; set; }
        public DbSet<Client> Client { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrdersFoodsRelationship> OrdersFoodsRelationships { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new FoodConfiguration()); 
            modelBuilder.ApplyConfiguration(new OrderConfiguration()); 
            modelBuilder.ApplyConfiguration(new ClientConfiguration()); 
            modelBuilder.ApplyConfiguration(new OrdersFoodsRelationshipConfiguration()); 
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(modelBuilder);
        }
    }
}
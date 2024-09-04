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
        public DbSet<Address> Address { get; set; }
        public DbSet<User> User { get; set; }
        public DbSet<Order> Orders { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new FoodConfiguration()); 
            modelBuilder.ApplyConfiguration(new OrderConfiguration()); 
            modelBuilder.ApplyConfiguration(new UserConfiguration()); 
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(modelBuilder);
        }
    }
}
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


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new FoodEntityConfiguration()); 
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(modelBuilder);
        }
    }
}
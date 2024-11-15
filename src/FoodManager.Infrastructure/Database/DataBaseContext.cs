using System.Reflection;
using FoodManager.API.Services;
using FoodManager.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace FoodManager.Infrastructure.Database
{
    public class DataBaseContext : DbContext
    {
        private readonly IHttpUserService _httpUserService;
        public DataBaseContext(DbContextOptions options, IHttpUserService httpUserService) : base(options)
        {
            _httpUserService = httpUserService;
        }

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
                var currentUser = await _httpUserService.VerifyTokenAsync();
                foreach (var entry in entriesData)
                {
                    if (currentUser is null)
                    {
                        throw new InvalidOperationException("Erro ao tentar realizar alterações sem operador vinculado.");
                    }

                    var entity = entry.Entity; 

                    switch (entry.State)
                    {
                        case EntityState.Added:
                            entity.CreatedAt = DateTime.UtcNow;
                            entity.CreatedByUserName = currentUser.Name;
                            entity.CreatedByUserId = currentUser.UserId;
                            entity.UpdatedByUserId = null;
                            entity.UpdatedByUserName = null;
                            break;
                        case EntityState.Modified:
                            entity.UpdatedByUserId = currentUser.Name;
                            entity.UpdatedByUserName = currentUser.UserId;
                            entity.UpdatedAt = DateTime.UtcNow;
                            break;
                    }
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
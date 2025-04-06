using System.Reflection;
using Api.Services;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Database
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
            modelBuilder.ApplyConfiguration(new CartConfiguration());
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(modelBuilder);
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var entriesData = ChangeTracker.Entries<BaseEntity>();
            UserInfoResponse? currentUser = null;

            try
            {
                currentUser = await _httpUserService.GetAuthenticatedUser();
            }
            catch
            {
                
            }

            foreach (var entry in entriesData)
            {
                var entity = entry.Entity;

                switch (entry.State)
                {
                    case EntityState.Added:
                        entity.CreatedAt = DateTime.UtcNow;
                        entity.CreatedByUserName = currentUser?.Name;
                        entity.CreatedByUserId = currentUser?.UserId;
                        entity.UpdatedByUserId = null;
                        entity.UpdatedByUserName = null;
                        break;

                    case EntityState.Modified:
                        entity.UpdatedAt = DateTime.UtcNow;
                        entity.UpdatedByUserId = currentUser?.UserId;
                        entity.UpdatedByUserName = currentUser?.Name;
                        break;
                }
            }

            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}

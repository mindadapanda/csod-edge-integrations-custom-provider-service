using Microsoft.EntityFrameworkCore;

namespace csod_edge_integrations_custom_provider_service.Models
{
    public class UserContext : DbContext
    {
        private static bool _created;
        public UserContext(DbContextOptions<UserContext> options)
            :base(options)
        {
            //manually manage your migrations in migrations folder
            if (_created) return;
            Database.Migrate();
            _created = true;
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Settings> Settings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().ForSqliteToTable("users");
            modelBuilder.Entity<Settings>().ForSqliteToTable("settings");
        }
    }
}

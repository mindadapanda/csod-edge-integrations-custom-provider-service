using Microsoft.EntityFrameworkCore;

namespace csod_edge_integrations_custom_provider_service.Models
{
    public class UserContext : DbContext
    {
        public UserContext(DbContextOptions<UserContext> options)
            :base(options)
        {

        }

        public DbSet<User> Users { get; set; }
    }
}

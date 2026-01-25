using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography.X509Certificates;
using SecureTaskTeamApi.Models;
namespace SecureTaskTeamApi.Data
{
    public class AppDBContext : DbContext
    {
        public AppDBContext(DbContextOptions<AppDBContext> options) : base(options) { }

        // DB        
        public DbSet<User> Users { get; set; }
        public DbSet<TaskItem> Tasks { get; set; }
    }
}

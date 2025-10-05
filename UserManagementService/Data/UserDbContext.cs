using Microsoft.EntityFrameworkCore;
using UserManagementService.Entities;

namespace UserManagementService.Data
{
    public class UserDbContext : DbContext
    {
        public UserDbContext(DbContextOptions<UserDbContext> options)
            : base(options)
        {
        }

        public DbSet<UserEntity> Users { get; set; }
    }
}
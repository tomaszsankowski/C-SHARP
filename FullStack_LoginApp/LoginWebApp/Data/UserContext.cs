using Microsoft.EntityFrameworkCore;
using LoginWebApp.Models;

namespace LoginWebApp.Data
{
    public class UserContext : DbContext
    {
        public UserContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<User> Users {  get; set; }
    }
}

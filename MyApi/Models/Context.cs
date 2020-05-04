using Microsoft.EntityFrameworkCore;

namespace MyApi.Models
{
    public class Context : DbContext
    {
        public Context(DbContextOptions<Context> options)
            : base(options)
        {
        }

        public DbSet<Item> Items { get; set; }
    }
}
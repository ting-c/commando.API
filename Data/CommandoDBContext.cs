using Microsoft.EntityFrameworkCore;

namespace CommandoAPI.Models
{
    public class CommandoDBContext : DbContext
    {
        public CommandoDBContext(DbContextOptions<CommandoDBContext> options) : base(options)
        {
        }

        public DbSet<CommandItem> CommandItems { get; set; }
    }
}

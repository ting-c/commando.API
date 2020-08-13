using Microsoft.EntityFrameworkCore;

namespace CommandoAPI.Models
{
    public class CommandContext : DbContext
    {
        public CommandContext(DbContextOptions<CommandContext> options) : base(options)
        {
        }

        public DbSet<CommandItem> CommandItems { get; set; }
    }
}

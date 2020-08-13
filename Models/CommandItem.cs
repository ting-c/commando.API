using System;
namespace CommandoAPI.Models
{
    public class CommandItem
    {
        public Guid Id { get; set; }
        public string Command { get; set; }
        public string Description { get; set; }
    }
}

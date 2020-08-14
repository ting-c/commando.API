using System;
namespace CommandoAPI.Models
{
    public class CommandItem
    {
        public CommandItem()
        {
            Id = Guid.NewGuid();
        }
       
        public Guid Id { get; }
        public string Command { get; set; }
        public string Description { get; set; }
    }
}

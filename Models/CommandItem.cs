using System;
using System.ComponentModel.DataAnnotations;

namespace CommandoAPI.Models
{
    public class CommandItem
    {
        public CommandItem()
        {
            Id = Guid.NewGuid();
        }

        [Key]
        public Guid Id { get; set; }
        public string Command { get; set; }
        public string Description { get; set; }
    }
}

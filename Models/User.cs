using System;
using System.ComponentModel.DataAnnotations;

namespace CommandoAPI.Models
{
    public class User
    {
        public User()
        {
            Id = Guid.NewGuid();
        }

        [Key]
        public Guid Id { get; set; }
        public string EmailAddress { get; set; }
        public string Password { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CommandoAPI.Models;

namespace CommandoAPI.Services
{
    public interface ICommandItemService
    {
        Task<List<CommandItem>> GetCommandItemsAsync();
    }
}

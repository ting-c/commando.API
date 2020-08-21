using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CommandoAPI.Models;

namespace CommandoAPI.Services
{
    public interface ICommandItemService
    {
        Task<List<CommandItem>> GetCommandItemsAsync();
        Task<CommandItem> GetCommandItemByIdAsync(Guid id);
        Task<CommandItem> FindCommandItemAsync(CommandItem commandItem);
        Task AddTaskAsync(CommandItem commandItem);
        Task DeleteTaskAsync(CommandItem commandItem);
    }
}

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CommandoAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace CommandoAPI.Services
{
    public class CommandItemService : ICommandItemService
    {
        private readonly CommandContext _context;

        public CommandItemService(CommandContext context)
        {
            _context = context;
        }

        public async Task<List<CommandItem>> GetCommandItemsAsync()
        {
            var items = await _context.CommandItems.ToListAsync();
            return items;
        }

        public async Task<CommandItem> GetCommandItemByIdAsync(Guid id)
        {
            var item = await _context.CommandItems.FirstOrDefaultAsync();
            return item;
        }

        public async Task<CommandItem> FindCommandItemAsync(CommandItem commandItem)
        {
            var SearchString = commandItem.Command;

            if (string.IsNullOrEmpty(SearchString))
            {
                return null;
            }

            var item = await _context.CommandItems.FirstOrDefaultAsync(item => item.Command == SearchString);
            return item;
        }

        public async Task AddTaskAsync(CommandItem commandItem)
        {
            _context.CommandItems.Add(commandItem);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteTaskAsync(CommandItem commandItem)
        {
            _context.CommandItems.Remove(commandItem);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateTaskAsync(Guid id, CommandItem commandItem)
        {
            var existingCommandItem = await GetCommandItemByIdAsync(id);
            existingCommandItem.Command = commandItem.Command;
            existingCommandItem.Description = commandItem.Description;
            await _context.SaveChangesAsync();
        }
    }
}

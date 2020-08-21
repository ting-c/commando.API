using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CommandoAPI.Models;

namespace CommandoAPI.Services
{
    public class FakeCommandItemService : ICommandItemService
    {
        private List<CommandItem> commandItems; 

        public FakeCommandItemService()
        {
            var item1 = new CommandItem
            {
                Command = "dotnet run",
                Description = "build and host app on localhost"
            };

            var item2 = new CommandItem
            {
                Command = "dotnet watch run",
                Description = "build and host app on localhost PLUS auto restart aftersave"
            };

            commandItems = new List<CommandItem> { item1, item2 };
        }

        // simulate an async task to get command items and return pre-made data
        public async Task<List<CommandItem>> GetCommandItemsAsync()
        {
            return await Task.FromResult(commandItems);
        }

        public async Task<CommandItem> GetCommandItemByIdAsync(Guid id)
        {
            return await Task.FromResult(commandItems.Find(item => item.Id == id));
        }

        public async Task<CommandItem> FindCommandItemAsync(CommandItem commandItem)
        {
            var SearchString = commandItem.Command;
            var item = commandItems.Find(item => item.Command == SearchString);
            return await Task.FromResult(item);
        }

        public Task AddTaskAsync(CommandItem commandItem)
        {
            commandItems.Add(commandItem);
            return Task.CompletedTask;
        }

        public Task DeleteTaskAsync(CommandItem commandItem)
        {
            commandItems.Remove(commandItem);
            return Task.CompletedTask;
        }
    }
}

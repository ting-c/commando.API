using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CommandoAPI.Models;

namespace CommandoAPI.Services
{
    public class FakeCommandItemService : ICommandItemService
    {
        // simulate an async task to get command items and return pre-made data
        public Task<List<CommandItem>> GetCommandItemsAsync()
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

            var list = new List<CommandItem> { item1, item2 };

            return Task.FromResult(list);
        }
    }
}

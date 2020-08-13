using System;
using System.Collections.Generic;
using CommandoAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace CommandoAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CommandoController : ControllerBase
    {
        private static List<CommandItem> CommandItems = new List<CommandItem>
        {
            new CommandItem
            {
                Command = "dotnet run",
                Description = "build and host app on localhost"
            },

            new CommandItem
            {
                Command = "dotnet watch run",
                Description = "build and host app on localhost PLUS auto restart aftersave"
            }
        };

        [HttpGet]
        public ActionResult<List<CommandItem>> Get()
        {
            return Ok(CommandItems);
        }

        [HttpGet]
        [Route("{command}")]
        public ActionResult<List<CommandItem>> Get(string command)
        {
            var commandItem = CommandItems.Find(item =>
                item.Command.Equals(command));

            if (commandItem == null)
            {
                return NotFound();
            } else
            {
                return Ok(commandItem);
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
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

        [HttpPost]
        public ActionResult Post(CommandItem commandItem)
        {
            var existingCommandItem = CommandItems.Find(item =>
                item.Command == commandItem.Command
            );

            if (existingCommandItem != null)
            {
                return Conflict("Command already exists");
            }
            else
            {
                CommandItems.Add(commandItem);
                var resourceUrl = Path.Combine(Request.Path.ToString(), Uri.EscapeUriString(commandItem.Command));
                // return an object with 201 Created status code along with the url in the location header and command item in the body
                return Created(resourceUrl, commandItem);
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using CommandoAPI.Models;
using CommandoAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace CommandoAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CommandoController : ControllerBase
    {
        private readonly ICommandItemService _commandItemService;

        public CommandoController(ICommandItemService commandItemService)
        {
            _commandItemService = commandItemService;
        }
       

        [HttpGet]
        public async Task<ActionResult<List<CommandItem>>> Get()
        {
            var commandItems = await _commandItemService.GetCommandItemsAsync();
            return Ok(commandItems);
        }

        [HttpGet]
        [Route("{command}")]
        public async Task<ActionResult<List<CommandItem>>> Get(string command)
        {
            var commandItems = await _commandItemService.GetCommandItemsAsync();
            var commandItem = commandItems.Find(item =>
                item.Command.Equals(command));

            if (commandItem == null)
            {
                return NotFound();
            } 

            return Ok(commandItem);
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<ActionResult<CommandItem>> Update(Guid id, CommandItem newCommandItem)
        {
            var commandItems = await _commandItemService.GetCommandItemsAsync();
            var commandItem = commandItems.Find(item =>
                item.Id.Equals(id));

            if (commandItem == null)
            {
                return NotFound();
            }

            commandItem.Command = newCommandItem.Command;
            commandItem.Description = newCommandItem.Description;
            return Ok(commandItem);
        }

        [HttpPost]
        public async Task<ActionResult> PostAsync(CommandItem commandItem)
        {
            var commandItems = await _commandItemService.GetCommandItemsAsync();
            var existingCommandItem = commandItems.Find(item =>
                item.Command == commandItem.Command);

            if (existingCommandItem != null)
            {
                return BadRequest("Command already exists");
            }
            else if (String.IsNullOrWhiteSpace(commandItem.Command))
            {
                return BadRequest("A command item needs a command!");
            }
            else if (String.IsNullOrWhiteSpace(commandItem.Description))
            {
                return BadRequest("A command item needs a description");
            }

            commandItems.Add(commandItem);
            var resourceUrl = Path.Combine(
                Request.Path.ToString(),
                Uri.EscapeUriString(commandItem.Command));

            return Created(resourceUrl, commandItem);
        }

        [HttpPut]
        public async Task<ActionResult> PutAsync(CommandItem commandItem)
        {
            var commandItems = await _commandItemService.GetCommandItemsAsync();
            var existingCommandItem = commandItems.Find(item =>
                item.Command == commandItem.Command);

            if (existingCommandItem == null)
            {
                return BadRequest("Cannot find the command, update failed");
            }
            else if (String.IsNullOrWhiteSpace(commandItem.Description))
            {
                return BadRequest("Cannot update command without a description");
            }

            existingCommandItem.Description = commandItem.Description;
            return Ok();
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<ActionResult> DeleteAsync(Guid id)
        {
            var commandItems = await _commandItemService.GetCommandItemsAsync();
            var existingCommandItem = commandItems.Find(item =>
                item.Id == id);

            if (existingCommandItem == null)
            {
                return NotFound();
            }

            commandItems.Remove(existingCommandItem);
            return NoContent();
        }
    }
}

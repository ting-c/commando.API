using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using CommandoAPI.Models;
using CommandoAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CommandoAPI.Controllers
{
    [Authorize]
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
        public async Task<ActionResult<List<CommandItem>>> GetAsync()
        {
            var commandItems = await _commandItemService.GetCommandItemsAsync();
            return commandItems;
        }

        [HttpPost]
        public async Task<ActionResult> PostAsync(CommandItem commandItem)
        {
            if (String.IsNullOrWhiteSpace(commandItem.Command))
            {
                return BadRequest("A command item needs a command!");
            }
            else if (String.IsNullOrWhiteSpace(commandItem.Description))
            {
                return BadRequest("A command item needs a description");
            }

            var existingCommandItem = await _commandItemService.FindCommandItemAsync(commandItem);

            if (existingCommandItem != null)
            {
                return BadRequest("Command already exists");
            }

            try
            {
                await _commandItemService.AddTaskAsync(commandItem);
            }
            catch 
            {
                return BadRequest("Failed to add item");
            }

            return CreatedAtAction("Add Command Item", commandItem);
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<ActionResult> PutAsync(Guid id, CommandItem commandItem)
        {
            var existingCommandItem = await _commandItemService.GetCommandItemByIdAsync(id);

            if (existingCommandItem == null)
            {
                return NotFound();
            }

            try
            {
                await _commandItemService.UpdateTaskAsync(id, commandItem);
            }
            catch
            {
                return BadRequest("Failed to update item");
            }

            return Ok();
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<ActionResult> DeleteAsync(Guid id)
        {
            var commandItem = await _commandItemService.GetCommandItemByIdAsync(id);

            if (commandItem == null)
            {
                return NotFound();
            }

            try
            {
                await _commandItemService.DeleteTaskAsync(commandItem);
            }
            catch
            {
                return BadRequest("Failed to delete item");
            }

            return Ok();
        }
    }
}

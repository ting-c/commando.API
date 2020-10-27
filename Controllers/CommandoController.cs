using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using CommandoAPI.Models;
using CommandoAPI.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

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
        public async Task<ActionResult<List<CommandItem>>> GetAsync()
        {
            await WriteOutIdentityInformation();
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

            return Ok();
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

        public async Task WriteOutIdentityInformation()
        {
            // get the saved identity token
            var identityToken = await HttpContext
                .GetTokenAsync(OpenIdConnectParameterNames.IdToken);

            // write it out
            Debug.WriteLine($"Identity token: {identityToken}");

            // write out the user claims
            foreach (var claim in User.Claims)
            {
                Debug.WriteLine($"Claim type: {claim.Type} - Claim value: {claim.Value}");
            }
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using Checkmark.API.Models;
using Checkmark.API.Services;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Checkmark.API.Controllers
{
  [ApiController]
  [Route("api/[controller]")]
  public class CheckmarkController : ControllerBase
  {
    private readonly ICheckmarkService _checkmarkService;
    public CheckmarkController(ICheckmarkService checkmarkService)
    {
      _checkmarkService = checkmarkService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CheckmarkItem>>> GetAll()
    {
      var items = await _checkmarkService.GetAllItemsAsync();
      return Ok(items);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<CheckmarkItem>> GetById(int id)
    {
      var item = await _checkmarkService.GetItemByIdAsync(id);

      return item == null ? NotFound() : Ok(item);
    }

    [HttpPost]
    public async Task<ActionResult<CheckmarkItem>> Create(CheckmarkItem item)
    {
      var createdItem = await _checkmarkService.CreateItemAsync(item);

      return CreatedAtAction(nameof(GetById), new { id = createdItem.Id }, createdItem);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<CheckmarkItem>> Update(int id, CheckmarkItem item)
    {
      if (id != item.Id)
      {
        return BadRequest();
      }
      var updatedItem = await _checkmarkService.UpdateItemAsync(item);

      return updatedItem != null ? NoContent() : NotFound();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
      var deleted = await _checkmarkService.DeleteItemAsync(id);
      return deleted ? NoContent() : NotFound();
    }

    [HttpGet("completed")]
    public async Task<ActionResult<IEnumerable<CheckmarkItem>>> GetCompletedItems()
    {
      var items = await _checkmarkService.GetCompletedItemsAsync();
      return Ok(items);
    }

    [HttpGet("pending")]
    public async Task<ActionResult<IEnumerable<CheckmarkItem>>> GetPending()
    {
      var items = await _checkmarkService.GetPendingItemsAsync();
      return Ok(items);
    }

    [HttpGet("priority/{priority}")]
    public async Task<ActionResult<IEnumerable<CheckmarkItem>>> GetByPriority(PriorityLevel priority)
    {
      var items = await _checkmarkService.GetItemsByPriorityAsync(priority);
      return Ok(items);
    }
  }
}
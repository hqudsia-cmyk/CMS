using Microsoft.AspNetCore.Mvc;
using CMS.Application.Interfaces;
using CMS.Domain.Entities;

namespace CMS.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MenuItemsController : ControllerBase
{
    private readonly IMenuItemService _menuItemService;

    public MenuItemsController(IMenuItemService menuItemService)
    {
        _menuItemService = menuItemService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<MenuItem>>> GetAll()
    {
        var items = await _menuItemService.GetAllMenuItemsAsync();
        return Ok(items);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<MenuItem>> GetById(int id)
    {
        var item = await _menuItemService.GetMenuItemByIdAsync(id);
        if (item == null)
            return NotFound();
        return Ok(item);
    }

    [HttpGet("hierarchy")]
    public async Task<ActionResult<IEnumerable<MenuItem>>> GetHierarchy()
    {
        var items = await _menuItemService.GetMenuHierarchyAsync();
        return Ok(items);
    }

    [HttpPost]
    public async Task<ActionResult<MenuItem>> Create(MenuItem menuItem)
    {
        var createdItem = await _menuItemService.CreateMenuItemAsync(menuItem);
        return CreatedAtAction(nameof(GetById), new { id = createdItem.Id }, createdItem);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, MenuItem menuItem)
    {
        var existingItem = await _menuItemService.GetMenuItemByIdAsync(id);
        if (existingItem == null)
            return NotFound();

        menuItem.Id = id;
        await _menuItemService.UpdateMenuItemAsync(menuItem);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var success = await _menuItemService.DeleteMenuItemAsync(id);
        if (!success)
            return NotFound();
        return NoContent();
    }
}

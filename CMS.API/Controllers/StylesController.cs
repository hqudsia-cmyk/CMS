using Microsoft.AspNetCore.Mvc;
using CMS.Application.Interfaces;
using CMS.Domain.Entities;

namespace CMS.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StylesController : ControllerBase
{
    private readonly IStyleConfigService _styleService;

    public StylesController(IStyleConfigService styleService)
    {
        _styleService = styleService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<StyleConfig>>> GetAll()
    {
        var styles = await _styleService.GetAllStylesAsync();
        return Ok(styles);
    }

    [HttpGet("{elementType}")]
    public async Task<ActionResult<StyleConfig>> GetByElementType(string elementType)
    {
        var style = await _styleService.GetStyleByElementTypeAsync(elementType);
        if (style == null)
            return NotFound();
        return Ok(style);
    }

    [HttpPost]
    public async Task<ActionResult<StyleConfig>> Create(StyleConfig style)
    {
        var createdStyle = await _styleService.CreateStyleAsync(style);
        return CreatedAtAction(nameof(GetByElementType), new { elementType = createdStyle.ElementType }, createdStyle);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, StyleConfig style)
    {
        style.Id = id;
        await _styleService.UpdateStyleAsync(style);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var success = await _styleService.DeleteStyleAsync(id);
        if (!success)
            return NotFound();
        return NoContent();
    }
}

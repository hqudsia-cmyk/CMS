using Microsoft.AspNetCore.Mvc;
using CMS.Application.Interfaces;
using CMS.Domain.Entities;

namespace CMS.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PageContentsController : ControllerBase
{
    private readonly IPageContentService _contentService;

    public PageContentsController(IPageContentService contentService)
    {
        _contentService = contentService;
    }

    [HttpGet("page/{pageId}")]
    public async Task<ActionResult<IEnumerable<PageContent>>> GetByPageId(int pageId)
    {
        var contents = await _contentService.GetPageContentsByPageIdAsync(pageId);
        return Ok(contents);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<PageContent>> GetById(int id)
    {
        var content = await _contentService.GetContentByIdAsync(id);
        if (content == null)
            return NotFound();
        return Ok(content);
    }

    [HttpPost]
    public async Task<ActionResult<PageContent>> Create(PageContent content)
    {
        var createdContent = await _contentService.CreateContentAsync(content);
        return CreatedAtAction(nameof(GetById), new { id = createdContent.Id }, createdContent);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, PageContent content)
    {
        var existingContent = await _contentService.GetContentByIdAsync(id);
        if (existingContent == null)
            return NotFound();

        content.Id = id;
        await _contentService.UpdateContentAsync(content);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var success = await _contentService.DeleteContentAsync(id);
        if (!success)
            return NotFound();
        return NoContent();
    }
}

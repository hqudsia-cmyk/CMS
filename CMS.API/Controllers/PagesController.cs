using Microsoft.AspNetCore.Mvc;
using CMS.Application.Interfaces;
using CMS.Domain.Entities;

namespace CMS.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PagesController : ControllerBase
{
    private readonly IPageService _pageService;

    public PagesController(IPageService pageService)
    {
        _pageService = pageService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CMSPage>>> GetAll()
    {
        var pages = await _pageService.GetAllPagesAsync();
        return Ok(pages);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<CMSPage>> GetById(int id)
    {
        var page = await _pageService.GetPageByIdAsync(id);
        if (page == null)
            return NotFound();
        return Ok(page);
    }

    [HttpGet("by-slug/{slug}")]
    public async Task<ActionResult<CMSPage>> GetBySlug(string slug)
    {
        var page = await _pageService.GetPageBySlugAsync(slug);
        if (page == null)
            return NotFound();
        return Ok(page);
    }

    [HttpPost]
    public async Task<ActionResult<CMSPage>> Create(CMSPage page)
    {
        var createdPage = await _pageService.CreatePageAsync(page);
        return CreatedAtAction(nameof(GetById), new { id = createdPage.Id }, createdPage);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, CMSPage page)
    {
        var existingPage = await _pageService.GetPageByIdAsync(id);
        if (existingPage == null)
            return NotFound();

        page.Id = id;
        await _pageService.UpdatePageAsync(page);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var success = await _pageService.DeletePageAsync(id);
        if (!success)
            return NotFound();
        return NoContent();
    }

    [HttpPost("{id}/increment-visitors")]
    public async Task<IActionResult> IncrementVisitors(int id)
    {
        await _pageService.IncrementVisitorCountAsync(id);
        return NoContent();
    }
}

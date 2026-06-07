using CMS.Application.Interfaces;
using CMS.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace CMS.Web.Pages.Admin.Pages;

public class EditModel : PageModel
{
    private readonly IPageService _pageService;
    private readonly IMenuItemService _menuItemService;
    private readonly IPageContentService _contentService;

    public EditModel(IPageService pageService, IMenuItemService menuItemService, IPageContentService contentService)
    {
        _pageService = pageService;
        _menuItemService = menuItemService;
        _contentService = contentService;
    }

    [BindProperty]
    public int PageId { get; set; }

    [BindProperty, Required]
    public string Title { get; set; } = string.Empty;

    [BindProperty, Required]
    public string Slug { get; set; } = string.Empty;

    [BindProperty]
    public int MenuItemId { get; set; }

    [BindProperty]
    public List<ContentBlockInput> Contents { get; set; } = new();

    public List<SelectListItem> MenuOptions { get; set; } = new();

    public class ContentBlockInput
    {
        public int Id { get; set; }
        public string ContentType { get; set; } = "text";
        public string Content { get; set; } = string.Empty;
        public int Order { get; set; }
    }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        var page = await _pageService.GetPageByIdAsync(id);
        if (page == null) return NotFound();

        PageId = page.Id;
        Title = page.Title;
        Slug = page.Slug;
        MenuItemId = page.MenuItemId;

        var contents = await _contentService.GetPageContentsByPageIdAsync(id);
        Contents = contents.OrderBy(c => c.Order).Select(c => new ContentBlockInput
        {
            Id = c.Id,
            ContentType = c.ContentType,
            Content = c.Content,
            Order = c.Order
        }).ToList();

        await LoadMenuOptionsAsync();
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            await LoadMenuOptionsAsync();
            return Page();
        }

        var page = await _pageService.GetPageByIdAsync(PageId);
        if (page == null) return NotFound();

        page.Title = Title;
        page.Slug = Slug;
        page.MenuItemId = MenuItemId;
        page.UpdatedAt = DateTime.UtcNow;
        await _pageService.UpdatePageAsync(page);

        // Delete existing content blocks then recreate
        var existing = await _contentService.GetPageContentsByPageIdAsync(PageId);
        foreach (var c in existing)
            await _contentService.DeleteContentAsync(c.Id);

        foreach (var c in Contents)
        {
            await _contentService.CreateContentAsync(new PageContent
            {
                PageId = PageId,
                ContentType = c.ContentType,
                Content = c.Content,
                Order = c.Order
            });
        }

        return RedirectToPage("/Admin/Pages");
    }

    private async Task LoadMenuOptionsAsync()
    {
        var items = await _menuItemService.GetAllMenuItemsAsync();
        MenuOptions = items.Select(m => new SelectListItem(m.Title, m.Id.ToString())).ToList();
    }
}

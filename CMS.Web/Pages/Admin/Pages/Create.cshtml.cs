using CMS.Application.Interfaces;
using CMS.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace CMS.Web.Pages.Admin.Pages;

public class CreateModel : PageModel
{
    private readonly IPageService _pageService;
    private readonly IMenuItemService _menuItemService;

    public CreateModel(IPageService pageService, IMenuItemService menuItemService)
    {
        _pageService = pageService;
        _menuItemService = menuItemService;
    }

    [BindProperty, Required]
    public string Title { get; set; } = string.Empty;

    [BindProperty, Required]
    public string Slug { get; set; } = string.Empty;

    [BindProperty, Required]
    public int MenuItemId { get; set; }

    [BindProperty]
    public List<ContentBlockInput> Contents { get; set; } = new();

    public List<SelectListItem> MenuOptions { get; set; } = new();

    public class ContentBlockInput
    {
        public string ContentType { get; set; } = "text";
        public string Content { get; set; } = string.Empty;
        public int Order { get; set; }
    }

    public async Task OnGetAsync()
    {
        await LoadMenuOptionsAsync();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            await LoadMenuOptionsAsync();
            return Page();
        }

        var page = new CMSPage
        {
            Title = Title,
            Slug = Slug,
            MenuItemId = MenuItemId,
            Contents = Contents.Select(c => new PageContent
            {
                ContentType = c.ContentType,
                Content = c.Content,
                Order = c.Order
            }).ToList()
        };

        await _pageService.CreatePageAsync(page);
        return RedirectToPage("/Admin/Pages");
    }

    private async Task LoadMenuOptionsAsync()
    {
        var items = await _menuItemService.GetAllMenuItemsAsync();
        MenuOptions = items.Select(m => new SelectListItem(m.Title, m.Id.ToString())).ToList();
    }
}

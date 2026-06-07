using CMS.Application.Interfaces;
using CMS.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace CMS.Web.Pages.Admin.Menu;

public class CreateModel : PageModel
{
    private readonly IMenuItemService _menuItemService;

    public CreateModel(IMenuItemService menuItemService)
    {
        _menuItemService = menuItemService;
    }

    [BindProperty]
    public MenuItemInput MenuItem { get; set; } = new();

    public List<SelectListItem> ParentOptions { get; set; } = new();

    public class MenuItemInput
    {
        [Required]
        public string Title { get; set; } = string.Empty;

        [Required]
        public string Slug { get; set; } = string.Empty;

        public int Order { get; set; }
        public int? ParentId { get; set; }
        public bool IsVisible { get; set; } = true;
    }

    public async Task OnGetAsync()
    {
        await LoadParentOptionsAsync();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            await LoadParentOptionsAsync();
            return Page();
        }

        var item = new CMS.Domain.Entities.MenuItem
        {
            Title = MenuItem.Title,
            Slug = MenuItem.Slug,
            Order = MenuItem.Order,
            ParentId = MenuItem.ParentId,
            IsVisible = MenuItem.IsVisible
        };

        await _menuItemService.CreateMenuItemAsync(item);
        return RedirectToPage("/Admin/Menu");
    }

    private async Task LoadParentOptionsAsync()
    {
        var items = await _menuItemService.GetRootMenuItemsAsync();
        ParentOptions = items.Select(m => new SelectListItem(m.Title, m.Id.ToString())).ToList();
    }
}

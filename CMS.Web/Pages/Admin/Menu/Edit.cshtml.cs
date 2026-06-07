using CMS.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace CMS.Web.Pages.Admin.Menu;

public class EditModel : PageModel
{
    private readonly IMenuItemService _menuItemService;

    public EditModel(IMenuItemService menuItemService)
    {
        _menuItemService = menuItemService;
    }

    [BindProperty]
    public MenuItemInput MenuItem { get; set; } = new();

    public List<SelectListItem> ParentOptions { get; set; } = new();

    public class MenuItemInput
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; } = string.Empty;

        [Required]
        public string Slug { get; set; } = string.Empty;

        public int Order { get; set; }
        public int? ParentId { get; set; }
        public bool IsVisible { get; set; } = true;
    }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        var item = await _menuItemService.GetMenuItemByIdAsync(id);
        if (item == null) return NotFound();

        MenuItem = new MenuItemInput
        {
            Id = item.Id,
            Title = item.Title,
            Slug = item.Slug,
            Order = item.Order,
            ParentId = item.ParentId,
            IsVisible = item.IsVisible
        };

        await LoadParentOptionsAsync(id);
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            await LoadParentOptionsAsync(MenuItem.Id);
            return Page();
        }

        var item = await _menuItemService.GetMenuItemByIdAsync(MenuItem.Id);
        if (item == null) return NotFound();

        item.Title = MenuItem.Title;
        item.Slug = MenuItem.Slug;
        item.Order = MenuItem.Order;
        item.ParentId = MenuItem.ParentId;
        item.IsVisible = MenuItem.IsVisible;

        await _menuItemService.UpdateMenuItemAsync(item);
        return RedirectToPage("/Admin/Menu");
    }

    private async Task LoadParentOptionsAsync(int excludeId)
    {
        var items = await _menuItemService.GetRootMenuItemsAsync();
        ParentOptions = items
            .Where(m => m.Id != excludeId)
            .Select(m => new SelectListItem(m.Title, m.Id.ToString()))
            .ToList();
    }
}

using Microsoft.AspNetCore.Mvc.RazorPages;
using CMS.Domain.Entities;
using CMS.Application.Interfaces;

namespace CMS.Web.Pages.Admin;

public class MenuModel : PageModel
{
    private readonly IMenuItemService _menuItemService;

    public List<MenuItem> MenuItems { get; set; } = new();

    public MenuModel(IMenuItemService menuItemService)
    {
        _menuItemService = menuItemService;
    }

    public async Task OnGetAsync()
    {
        var items = await _menuItemService.GetAllMenuItemsAsync();
        MenuItems = items.OrderBy(m => m.Order).ToList();
    }
}

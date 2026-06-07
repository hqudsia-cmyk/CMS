using Microsoft.AspNetCore.Mvc.RazorPages;
using CMS.Application.Interfaces;

namespace CMS.Web.Pages.Admin;

public class DashboardModel : PageModel
{
    private readonly IPageService _pageService;
    private readonly IMenuItemService _menuItemService;

    public int TotalPages { get; set; }
    public int TotalMenuItems { get; set; }

    public DashboardModel(IPageService pageService, IMenuItemService menuItemService)
    {
        _pageService = pageService;
        _menuItemService = menuItemService;
    }

    public async Task OnGetAsync()
    {
        var pages = await _pageService.GetAllPagesAsync();
        TotalPages = pages.Count();

        var menuItems = await _menuItemService.GetAllMenuItemsAsync();
        TotalMenuItems = menuItems.Count();
    }
}

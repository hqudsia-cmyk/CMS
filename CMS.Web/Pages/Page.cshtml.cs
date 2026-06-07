using CMS.Domain.Entities;
using CMS.Application.Interfaces;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CMS.Web.Pages;

public class CmsPageModel : PageModel
{
    private readonly IPageService _pageService;
    private readonly IMenuItemService _menuItemService;
    private readonly IPageContentService _contentService;

    public string? Slug { get; set; }
    public List<MenuItem> MenuItems { get; set; } = new();
    public CMSPage? CurrentPage { get; set; }
    public List<PageContent> PageContents { get; set; } = new();

    public CmsPageModel(IPageService pageService, IMenuItemService menuItemService, IPageContentService contentService)
    {
        _pageService = pageService;
        _menuItemService = menuItemService;
        _contentService = contentService;
    }

    public async Task OnGetAsync(string slug)
    {
        Slug = slug;

        // Get root menu items
        var items = await _menuItemService.GetRootMenuItemsAsync();
        MenuItems = items.ToList();

        // Get page
        CurrentPage = await _pageService.GetPageBySlugAsync(slug);
        if (CurrentPage != null)
        {
            // Increment visitor count
            await _pageService.IncrementVisitorCountAsync(CurrentPage.Id);

            // Get page contents
            var contents = await _contentService.GetPageContentsByPageIdAsync(CurrentPage.Id);
            PageContents = contents.ToList();
        }
    }
}

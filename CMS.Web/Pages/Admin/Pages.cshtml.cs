using Microsoft.AspNetCore.Mvc.RazorPages;
using CMS.Domain.Entities;
using CMS.Application.Interfaces;

namespace CMS.Web.Pages.Admin;

public class PagesModel : PageModel
{
    private readonly IPageService _pageService;

    public List<CMSPage> Pages { get; set; } = new();

    public PagesModel(IPageService pageService)
    {
        _pageService = pageService;
    }

    public async Task OnGetAsync()
    {
        var pages = await _pageService.GetAllPagesAsync();
        Pages = pages.OrderByDescending(p => p.CreatedAt).ToList();
    }
}

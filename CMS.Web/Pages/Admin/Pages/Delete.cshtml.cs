using CMS.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CMS.Web.Pages.Admin.Pages;

public class DeleteModel : PageModel
{
    private readonly IPageService _pageService;

    public DeleteModel(IPageService pageService)
    {
        _pageService = pageService;
    }

    public int PageId { get; set; }
    public string PageTitle { get; set; } = string.Empty;

    public async Task<IActionResult> OnGetAsync(int id)
    {
        var page = await _pageService.GetPageByIdAsync(id);
        if (page == null) return NotFound();

        PageId = page.Id;
        PageTitle = page.Title;
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(int id)
    {
        await _pageService.DeletePageAsync(id);
        return RedirectToPage("/Admin/Pages");
    }
}

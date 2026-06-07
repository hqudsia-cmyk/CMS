using CMS.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CMS.Web.Pages.Admin.Menu;

public class DeleteModel : PageModel
{
    private readonly IMenuItemService _menuItemService;

    public DeleteModel(IMenuItemService menuItemService)
    {
        _menuItemService = menuItemService;
    }

    public int ItemId { get; set; }
    public string ItemTitle { get; set; } = string.Empty;

    public async Task<IActionResult> OnGetAsync(int id)
    {
        var item = await _menuItemService.GetMenuItemByIdAsync(id);
        if (item == null) return NotFound();

        ItemId = item.Id;
        ItemTitle = item.Title;
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(int id)
    {
        await _menuItemService.DeleteMenuItemAsync(id);
        return RedirectToPage("/Admin/Menu");
    }
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CMS.Domain.Entities;
using CMS.Application.Interfaces;

namespace CMS.Web.Pages.Admin;

public class StylesModel : PageModel
{
    private readonly IStyleConfigService _styleService;

    public List<StyleConfig> Styles { get; set; } = new();

    public StylesModel(IStyleConfigService styleService)
    {
        _styleService = styleService;
    }

    public async Task OnGetAsync()
    {
        var styles = await _styleService.GetAllStylesAsync();
        Styles = styles.ToList();
    }

    public async Task<IActionResult> OnPostCreateAsync(string elementType, string cssStyles)
    {
        if (string.IsNullOrWhiteSpace(elementType) || string.IsNullOrWhiteSpace(cssStyles))
        {
            return BadRequest();
        }

        var style = new StyleConfig
        {
            ElementType = elementType,
            CssStyles = cssStyles
        };

        await _styleService.CreateStyleAsync(style);
        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostUpdateAsync(int styleId, string cssStyles)
    {
        var all = await _styleService.GetAllStylesAsync();
        var style = all.FirstOrDefault(s => s.Id == styleId);
        if (style == null)
            return NotFound();

        style.CssStyles = cssStyles;
        style.UpdatedAt = DateTime.UtcNow;
        await _styleService.UpdateStyleAsync(style);
        return RedirectToPage();
    }
}

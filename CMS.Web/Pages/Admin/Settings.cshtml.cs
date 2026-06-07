using CMS.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace CMS.Web.Pages.Admin;

public class SettingsModel : PageModel
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;

    public SettingsModel(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }

    [BindProperty]
    [Required, DataType(DataType.Password), Display(Name = "Current Password")]
    public string CurrentPassword { get; set; } = string.Empty;

    [BindProperty]
    [Required, MinLength(8), DataType(DataType.Password), Display(Name = "New Password")]
    public string NewPassword { get; set; } = string.Empty;

    [BindProperty]
    [Required, DataType(DataType.Password), Compare(nameof(NewPassword)), Display(Name = "Confirm Password")]
    public string ConfirmPassword { get; set; } = string.Empty;

    public void OnGet() { }

    public async Task<IActionResult> OnPostChangePasswordAsync()
    {
        if (!ModelState.IsValid) return Page();

        var user = await _userManager.GetUserAsync(User);
        if (user == null) return NotFound();

        var result = await _userManager.ChangePasswordAsync(user, CurrentPassword, NewPassword);
        if (!result.Succeeded)
        {
            foreach (var error in result.Errors)
                ModelState.AddModelError(string.Empty, error.Description);
            return Page();
        }

        await _signInManager.RefreshSignInAsync(user);
        TempData["Success"] = "Password updated successfully.";
        return RedirectToPage();
    }
}

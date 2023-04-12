using Microsoft.AspNetCore.Authorization;
using BlogSite.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BlogSite.Pages.Authentication
{
    public class Logout : PageModel
    {
        private readonly SignInManager<AppUser> _signInManager;

        public Logout(SignInManager<AppUser> signInManager)
        {
            _signInManager = signInManager;
        }

        public async Task<IActionResult> OnGet()
        {
            await _signInManager.SignOutAsync();
            return Redirect("/");
        }
    }
}

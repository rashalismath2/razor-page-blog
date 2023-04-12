
using System;
using BlogSite.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BlogSite.Pages.Authentication
{
    public class Login : PageModel
    {
        private readonly SignInManager<AppUser> _signInManager;

        public Login(SignInManager<AppUser> signInManager)
        {
            _signInManager = signInManager;
        }

        [BindProperty]
        public LoginVm Input { get; set; }
        [TempData]
        public string ErrorMessage { get; set; }


        public async Task<IActionResult> OnGetAsync()
        {
            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, ErrorMessage);
            }
            if (this.User.Identity.IsAuthenticated) {
                return Redirect("/");
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(Input.Email, Input.Password,true, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    if (string.IsNullOrEmpty(returnUrl)) {
                        return Redirect("/");
                    }
                    return Redirect(returnUrl);
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return Page();
                }
            }
            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}

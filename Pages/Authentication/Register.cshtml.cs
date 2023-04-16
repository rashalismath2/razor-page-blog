
using Microsoft.AspNetCore.Authentication;
using BlogSite.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace BlogSite.Pages.Authentication
{
    public class Register : PageModel
    {
        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly IUserStore<AppUser> _userStore;
        private readonly IWebHostEnvironment _hostenvironment;
        public Register(
            UserManager<AppUser> userManager,
            IUserStore<AppUser> userStore,
            SignInManager<AppUser> signInManager,
            IWebHostEnvironment webHostEnvironment
            )
        {
            _userManager = userManager;
            _userStore = userStore;
            _signInManager = signInManager;
            _hostenvironment = webHostEnvironment;
        }

        [BindProperty]
        public SignupVm Input { get; set; }


        public async Task<IActionResult> OnGetAsync()
        {
            if (this.User.Identity.IsAuthenticated)
            {
                return Redirect("/");
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var user = CreateUser();

                user.FullName = Input.FullName;
                user.Email = Input.Email;
                user.Bio = Input.Bio;
                await _userStore.SetUserNameAsync(user, Input.Email, CancellationToken.None);

                var result = await _userManager.CreateAsync(user, Input.Password);
                await _userManager.AddClaimAsync(user, new Claim("FullName", user.FullName));
                await _userManager.AddClaimAsync(user, new Claim("Id", user.Id));

                if (result.Succeeded)
                {
                    if (Input.ProfilePic is not null && Input.ProfilePic.Length > 0)
                    {
                        var folder = Path.Combine(_hostenvironment.WebRootPath, "UploadedFiles");
                        if (!Directory.Exists(folder))
                        {
                            Directory.CreateDirectory(folder);
                        }
                        var picName = Guid.NewGuid().ToString() + "-" + Input.ProfilePic.FileName;
                        var picPathName = Path.Combine(folder, picName);
                        using (var stream = new FileStream(picPathName, FileMode.Create))
                        {
                            await Input.ProfilePic.CopyToAsync(stream);
                        }
                        user.ProfileUrl = "https://" + HttpContext.Request.Host.Value + "/UploadedFiles/" + picName;
                        await _userStore.UpdateAsync(user, CancellationToken.None);
                    }

                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return Redirect("/");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }

        private AppUser CreateUser()
        {
            try
            {
                return Activator.CreateInstance<AppUser>();
            }
            catch
            {
                throw new InvalidOperationException($"Can't create an instance of '{nameof(AppUser)}'. " +
                    $"Ensure that '{nameof(AppUser)}' is not an abstract class and has a parameterless constructor, or alternatively " +
                    $"override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
            }
        }
    }
}

using BlogSite.Models;
using BlogSite.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System.Security.Claims;

namespace BlogSite.Pages.Profile
{
    public class EditModel : PageModel
    {
        private readonly AppDbContext dbContext;


        [BindProperty]
        public UpdateProfileRequestVm Input { get; set; } = new();
        [BindProperty(SupportsGet = true)]
        public string userId { get; set; }
        public AppUser TargetUser { get; set; }

        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly IUserStore<AppUser> _userStore;
        private readonly IWebHostEnvironment _hostenvironment;
        public EditModel(
            UserManager<AppUser> userManager,
            IUserStore<AppUser> userStore,
            SignInManager<AppUser> signInManager,
            IWebHostEnvironment webHostEnvironment,
            AppDbContext dbContext
            )
        {
            _userManager = userManager;
            _userStore = userStore;
            _signInManager = signInManager;
            _hostenvironment = webHostEnvironment;
            this.dbContext = dbContext;
        }

        public async Task<IActionResult> OnGet()
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                return Redirect("/");
            }
            TargetUser = await dbContext.Users
                    .Include(u => u.Followers)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (User.Claims.ToList().FirstOrDefault((c) => c.Type == "Id").Value != TargetUser.Id)
            {
                return Redirect("/");
            }

            Input.Email = TargetUser.Email;
            Input.FullName = TargetUser.FullName;
            Input.Bio = TargetUser.Bio;


            if (User is null)
            {
                return Redirect("/");
            }
            return Page();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            TargetUser = await dbContext.Users
                  .Include(u => u.Followers)
              .FirstOrDefaultAsync(u => u.Id == userId);

            if (ModelState.IsValid)
            {
                if (!string.IsNullOrWhiteSpace(Input.CurrentPassword) && !string.IsNullOrWhiteSpace(Input.Password))
                {
                    var changePasswordResult = await _userManager.ChangePasswordAsync(TargetUser, Input.CurrentPassword, Input.Password);
                    if (!changePasswordResult.Succeeded)
                    {
                        ModelState.AddModelError("CurrentPassword", "Password Error");
                        return Page();
                    }
                }

                TargetUser.FullName = Input.FullName;
                TargetUser.Bio = Input.Bio;

                var result = await _userStore.UpdateAsync(TargetUser, CancellationToken.None);
                var claims = await _userManager.GetClaimsAsync(TargetUser);
                await _userManager.ReplaceClaimAsync(TargetUser, claims.FirstOrDefault(c => c.Type == "FullName"), new Claim("FullName", Input.FullName));


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
                        TargetUser.ProfileUrl = "https://" + HttpContext.Request.Host.Value + "/UploadedFiles/" + picName;
                        await _userStore.UpdateAsync(TargetUser, CancellationToken.None);
                    }
                    await _signInManager.SignOutAsync();
                    await _signInManager.SignInAsync(TargetUser, isPersistent: false);
                    return Redirect("/profile?userId=" + TargetUser.Id);
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return Page();
        }
    }
}

using BlogSite.Models;
using BlogSite.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace BlogSite.Pages.Profile
{
    public class IndexModel : PageModel
    {
        private readonly AppDbContext dbContext;


        [BindProperty(SupportsGet = true)]
        public string userId { get; set; }
        public AppUser User { get; set; }
        public IndexModel(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<IActionResult> OnGet()
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                return Redirect("/");
            }
            User = await dbContext.Users
                .Include(u=>u.Articles.Where(a=>a.IsAllowed))
                .Include(u=>u.Followers)
                .FirstOrDefaultAsync(u => u.Id == userId);
            if (User is null)
            {
                return Redirect("/");
            }
            return Page();
        }
    }
}

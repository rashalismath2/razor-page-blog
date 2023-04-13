using BlogSite.Models;
using BlogSite.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace BlogSite.Pages.Posts
{
    public class DetailsModel : PageModel
    {
        private readonly AppDbContext _dbContext;
        public Post Post { get; set; }

        public DetailsModel(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<IActionResult> OnGet(int id)
        {
            var post = await _dbContext.Posts.Include(p => p.AppUser).FirstOrDefaultAsync(p => p.Id == id);
            if (post is null)
            {
                return Redirect("/");
            }
            Post = post;
            if (post.IsAllowed)
            {
                return Page();
            }

            if (User.Identity.IsAuthenticated && User.Claims.ToList().FirstOrDefault((c) => c.Type == "Id").Value == post.AppUserId)
            {
                return Page();
            }
            return Redirect("/");
        }

    }
}

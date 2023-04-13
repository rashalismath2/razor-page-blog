using BlogSite.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace BlogSite.Pages.Posts
{
    public class DeleteModel : PageModel
    {
        private readonly AppDbContext _dbContext;

        public DeleteModel(AppDbContext dbContext)
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
            if (User.Identity.IsAuthenticated && User.Claims.ToList().FirstOrDefault((c) => c.Type == "Id").Value == post.AppUserId)
            {
                _dbContext.Remove(post);
                await _dbContext.SaveChangesAsync();
                return Redirect("/");
            }
            return Redirect($"./details/{post.Id}");

        }
    }
}

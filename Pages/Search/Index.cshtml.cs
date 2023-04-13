using BlogSite.Models;
using BlogSite.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace BlogSite.Pages.Search
{
    public class IndexModel : PageModel
    {
        private readonly AppDbContext _dbContext;

        [BindProperty(SupportsGet = true)]
        public string title { get; set; }
        public List<Post> Posts { get; set; } = new();
        public IndexModel(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<IActionResult> OnGet()
        {
            if (!string.IsNullOrWhiteSpace(title))
            {
                var posts = await _dbContext.Posts.Where(p => p.Title.ToLower().Contains(title.ToLower()))
                    .Include(p => p.AppUser)
                    .ToListAsync();
                Posts = posts;
            }

            return Page();
        }
    }
}

using BlogSite.Models;
using BlogSite.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;

namespace BlogSite.Pages.Search
{
    public class IndexModel : PageModel
    {
        private readonly AppDbContext _dbContext;

        [BindProperty(SupportsGet = true)]
        public string title { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? tag { get; set; }
        public List<Post> Posts { get; set; } = new();
        public List<Tags> Tags { get; set; } = new();
        public IndexModel(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<IActionResult> OnGet()
        {
            var query = _dbContext.Posts.Include(p => p.AppUser)
                        .Include(p => p.Likes)
                        .Include(p=>p.Tag)
                        .Include(p => p.Comments).AsQueryable();
            if (!string.IsNullOrEmpty(tag))
            {
                query = query.Where(p => p.Tag != null && p.Tag.Title.ToLower() == tag.ToLower());
            }
            if (!string.IsNullOrWhiteSpace(title))
            {
                query = query.Where(p => p.Title.ToLower().Contains(title.ToLower()) && p.IsAllowed);

            }
            if (!string.IsNullOrEmpty(tag) || !string.IsNullOrWhiteSpace(title))
            {

                Posts = await query
                        .ToListAsync();
            }
            Tags = await _dbContext.Tags.ToListAsync();
            return Page();
        }
    }
}

using BlogSite.Models;
using BlogSite.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace BlogSite.Pages
{
    public class IndexModel : PageModel
    {
        private readonly AppDbContext _dbContext;
        public List<Post> Posts { get; set; } = new();
        public IndexModel(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<IActionResult> OnGet()
        {
            Posts = await _dbContext.Posts
                                .Include(p => p.AppUser)
                                .Where(p => p.IsAllowed)
                                .ToListAsync();
            return Page();
        }
    }
}
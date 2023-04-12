using BlogSite.Models;
using BlogSite.Repository;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace BlogSite.Pages
{
    public class IndexModel : PageModel
    {
        private readonly AppDbContext _db;
        public List<Post> Posts { get; set; }
        public IndexModel(AppDbContext db)
        {
            _db = db;
        }

        public async Task OnGet()
        {
            //Posts = await _db.Posts.ToListAsync();
        }
    }
}
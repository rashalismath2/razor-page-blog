using BlogSite.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BlogSite.Pages.Posts
{
    public class CreateModel : PageModel
    {
        public Post Post { get; set; }
        public void OnGet()
        {
        }
    }
}

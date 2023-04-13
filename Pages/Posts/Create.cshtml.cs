using BlogSite.Models;
using BlogSite.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BlogSite.Pages.Posts
{
    [Authorize]
    public class CreateModel : PageModel
    {
        private readonly AppDbContext _dbContext;
        private readonly IWebHostEnvironment webHostEnvironment;

        public CreateModel(AppDbContext dbContext, IWebHostEnvironment webHostEnvironment)
        {
            _dbContext = dbContext;
            this.webHostEnvironment = webHostEnvironment;
        }

        [BindProperty]
        public CreatePostVm Input { get; set; }
        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPost()
        {
            if (ModelState.IsValid)
            {
                Input.Body = Input.Body.Replace("img src=\"..", "img src=\"https://localhost:7020/");

                var post = new Post
                {
                    Title = Input.Title,
                    Body = Input.Body,
                    CreatedDate = DateTime.Now,
                    AppUserId = User.Claims.ToList().FirstOrDefault((c) => c.Type == "Id").Value
                };

                if (Input.CoverImage is not null && Input.CoverImage.Length > 0)
                {
                    var folder = Path.Combine(webHostEnvironment.WebRootPath, "UploadedFiles");
                    if (!Directory.Exists(folder))
                    {
                        Directory.CreateDirectory(folder);
                    }
                    var picName = Guid.NewGuid().ToString() + "-" + Input.CoverImage.FileName;
                    var picPathName = Path.Combine(folder, picName);
                    using (var stream = new FileStream(picPathName, FileMode.Create))
                    {
                        await Input.CoverImage.CopyToAsync(stream);
                    }
                    post.CoverImage = "https://" + HttpContext.Request.Host.Value + "/UploadedFiles/" + picName;
                }
                else
                {
                    ModelState.AddModelError("CoverImage","Invalid image");
                    return Page();
                }

                _dbContext.Posts.Add(post);
                await _dbContext.SaveChangesAsync();

                return Redirect($"./details/{post.Id}");
            }
            return Page();
        }
    }
}

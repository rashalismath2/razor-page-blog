using BlogSite.Models;
using BlogSite.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text;

namespace BlogSite.Pages.Posts
{
    [Authorize]
    public class CreateModel : PageModel
    {
        private readonly AppDbContext _dbContext;
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly HttpClient http;
        private readonly string aiEndpoint;

        public CreateModel(AppDbContext dbContext, IWebHostEnvironment webHostEnvironment, IConfiguration configs)
        {
            _dbContext = dbContext;
            this.webHostEnvironment = webHostEnvironment;
            this.http = new HttpClient();
            aiEndpoint = new Uri(configs["Ai_Endpoint"]).ToString();
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

                var tag = await _dbContext.Tags.FirstOrDefaultAsync(tag => tag.Title.ToLower() == Input.Tag.ToLower());
                if (tag is null)
                {
                    tag = new Tags { Title = Input.Title };
                    _dbContext.Tags.Add(tag);
                }

                var post = new Post
                {
                    Title = Input.Title,
                    Body = Input.Body,
                    CreatedDate = DateTime.Now,
                    AppUserId = User.Claims.ToList().FirstOrDefault((c) => c.Type == "Id").Value,
                    IsAllowed = true,
                    Tag = tag
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
                    ModelState.AddModelError("CoverImage", "Invalid image");
                    return Page();
                }


                var jsonString = JsonConvert.SerializeObject(new AIEndpointRequestBody(post.Title, post.Body));
                var stringContent = new StringContent(jsonString, Encoding.UTF8, "application/json");

                //HttpResponseMessage response = await http.PostAsync(aiEndpoint, stringContent);
                //var httpResonse = "";
                //if (response.IsSuccessStatusCode)
                //{
                //    httpResonse = await response.Content.ReadAsStringAsync();
                //}
                //else
                //{
                //    return Page();
                //}
                //var responseObject = JsonConvert.DeserializeObject<AIEndpointResponseBody>(httpResonse);
                //var isPostRejected = responseObject.IsTItleContainsHate == "hate" || responseObject.IsBodyContainsHate == "hate";

                //if (isPostRejected)
                //{
                //    var hateReason = "";
                //    if (responseObject.IsTItleContainsHate == "hate")
                //    {
                //        hateReason = "Title contains Hateful contents.";
                //    }
                //    if (responseObject.IsBodyContainsHate == "hate")
                //    {
                //        hateReason = hateReason + " Body contains Hateful contents.";
                //    }
                //    post.IsAllowed = false;
                //    post.NotAllowedReason = hateReason;
                //}

                _dbContext.Posts.Add(post);
                await _dbContext.SaveChangesAsync();

                return Redirect($"./details/{post.Id}");
            }
            return Page();
        }
    }
}

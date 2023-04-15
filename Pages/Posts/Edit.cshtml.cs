using BlogSite.Models;
using BlogSite.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using static System.Net.WebRequestMethods;
using System.Text;

namespace BlogSite.Pages.Posts
{
    public class EditModel : PageModel
    {
        private readonly AppDbContext _dbContext;
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly HttpClient http;
        private readonly string aiEndpoint;

        public EditModel(AppDbContext dbContext, IWebHostEnvironment webHostEnvironment, IConfiguration configs)
        {
            _dbContext = dbContext;
            this.webHostEnvironment = webHostEnvironment;
            this.http = new HttpClient();
            aiEndpoint = new Uri(configs["Ai_Endpoint"]).ToString();
        }

        [BindProperty]
        public UpdatePostvm Input { get; set; }


        public async Task<IActionResult> OnGet(int id)
        {
            var post = await _dbContext.Posts.FirstOrDefaultAsync(p => p.Id == id);
            if (post is null)
            {
                return Redirect("/");
            }

            Input = new UpdatePostvm
            {
                Title = post.Title,
                Body = post.Body,
                ExistingCoverImage=post.CoverImage
            };

            if (User.Identity.IsAuthenticated && User.Claims.ToList().FirstOrDefault((c) => c.Type == "Id").Value == post.AppUserId)
            {
                return Page();
            }

            return Redirect("/");
        }

        public async Task<IActionResult> OnPost(int id)
        {
            if (ModelState.IsValid)
            {
                var post = await _dbContext.Posts.FirstOrDefaultAsync(p => p.Id == id);
                if (post is null)
                {
                    return Redirect("/");
                }

                Input.Body = Input.Body.Replace("img src=\"..", "img src=\"https://localhost:7020/");

                post.Title = Input.Title;
                post.Body = Input.Body;
                post.CreatedDate = DateTime.Now;

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


                var jsonString = JsonConvert.SerializeObject(new AIEndpointRequestBody(post.Title, post.Body));
                var stringContent = new StringContent(jsonString, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await http.PostAsync(aiEndpoint, stringContent);
                var httpResonse = "";
                if (response.IsSuccessStatusCode)
                {
                    httpResonse = await response.Content.ReadAsStringAsync();
                }
                else
                {
                    return Page();
                }
                var responseObject = JsonConvert.DeserializeObject<AIEndpointResponseBody>(httpResonse);
                var isPostRejected = responseObject.IsTItleContainsHate == "hate" || responseObject.IsBodyContainsHate == "hate";

                if (isPostRejected)
                {
                    var hateReason = "";
                    if (responseObject.IsTItleContainsHate == "hate")
                    {
                        hateReason = "Title contains Hateful contents.";
                    }
                    if (responseObject.IsBodyContainsHate == "hate")
                    {
                        hateReason = hateReason + " Body contains Hateful contents.";
                    }
                    post.IsAllowed = false;
                    post.NotAllowedReason = hateReason;
                }


                _dbContext.Posts.Update(post);
                await _dbContext.SaveChangesAsync();

                return Redirect($"/Posts/Details/{post.Id}");
            }
            return Page();
        }
    }
}

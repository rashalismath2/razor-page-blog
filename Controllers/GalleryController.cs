using BlogSite.Migrations;
using BlogSite.Models;
using BlogSite.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;

namespace BlogSite.Controllers
{
    [Route("api/gallery")]
    [ApiController]
    [Authorize]
    public class GalleryController : ControllerBase
    {
        private readonly AppDbContext _dbContext;
        private readonly IWebHostEnvironment _hostenvironment;

        public GalleryController(AppDbContext dbContex, IWebHostEnvironment hostEnvironment)
        {
            _dbContext = dbContex;
            _hostenvironment=hostEnvironment;
        }
        [HttpGet]
        public async Task<ActionResult<List<GalleryImages>>> Get()
        {
            var userId = User.Claims.ToList().FirstOrDefault((c) => c.Type == "Id").Value;
            var galleryImages = await _dbContext.AppUsers.Where(u => u.Id == userId)
                .Include(u => u.GalleryImages)
                .Select(u => u.GalleryImages)
                .FirstOrDefaultAsync();

            return Ok(galleryImages);
        }
        [HttpPost]
        public async Task<ActionResult<GalleryImages>> Post([FromForm] IFormFile image)
        {
            if (image is not null && image.Length > 0)
            {
                var folder = Path.Combine(_hostenvironment.WebRootPath, "UploadedFiles");
                if (!Directory.Exists(folder))
                {
                    Directory.CreateDirectory(folder);
                }
                var picName = Guid.NewGuid().ToString() + "-" + image.FileName;
                var picPathName = Path.Combine(folder, picName);
                using (var stream = new FileStream(picPathName, FileMode.Create))
                {
                    await image.CopyToAsync(stream);
                }

                var userId = User.Claims.ToList().FirstOrDefault((c) => c.Type == "Id").Value;
                var user = await _dbContext.AppUsers.Where(u => u.Id == userId)
                .FirstOrDefaultAsync();

                var galleryImage = new GalleryImages { Url = "https://" + HttpContext.Request.Host.Value + "/UploadedFiles/" + picName };
                user.GalleryImages = new List<GalleryImages> {
                    galleryImage
                };

                _dbContext.Update(user);
                await _dbContext.SaveChangesAsync();

                return Ok(galleryImage);
            }

            return BadRequest("Image is not valid");
        }

    }
}

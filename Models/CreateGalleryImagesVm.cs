using Microsoft.Build.Framework;

namespace BlogSite.Models
{
    public class CreateGalleryImagesVm
    {
        [Required]
        public IFormFile Image { get; set; }
    }
}

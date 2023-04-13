using System.ComponentModel.DataAnnotations;

namespace BlogSite.Models
{
    public class CreatePostVm
    {
        [Required]
        [MinLength(3)]
        public string Title { get; set; }
        [Required]
        [MinLength(10)]
        public string Body { get; set; }
        [Required]
        public IFormFile CoverImage { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace BlogSite.Models
{
    public class UpdatePostvm
    {
        [Required]
        [MinLength(3)]
        public string Title { get; set; }
        [Required]
        [MinLength(10)]
        public string Body { get; set; }
        public IFormFile? CoverImage { get; set; }
        public string? ExistingCoverImage { get; set; }
    }
}

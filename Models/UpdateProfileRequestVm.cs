using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace BlogSite.Models
{
    public class UpdateProfileRequestVm
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
        [Required]
        [Display(Name = "FullName")]
        public string FullName { get; set; }

        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string? Password { get; set; }
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "CurrentPassword")]
        public string? CurrentPassword { get; set; }
        public IFormFile? ProfilePic { get; set; }
        [Required]
        [MinLength(3)]
        [MaxLength(60)]
        public string Bio { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace BlogSite.Models
{
    public class SignupVm
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }       
        [Required]
        [Display(Name = "FullName")]
        public string FullName { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }
        public IFormFile? ProfilePic { get; set; }
    }
}

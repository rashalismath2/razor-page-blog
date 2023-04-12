using System.ComponentModel.DataAnnotations;

namespace BlogSite.Models
{
    public class Post
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MinLength(3)]
        public string TItle { get; set; }
        [Required]
        [MinLength(10)]
        public string Body { get; set; }
        public DateTime CreatedDate { get; set; }
        //public List<string> Images { get; set; }
    }
}

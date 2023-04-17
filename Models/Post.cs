using System.ComponentModel.DataAnnotations;

namespace BlogSite.Models
{
    public class Post
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MinLength(3)]
        public string Title { get; set; }
        [Required]
        [MinLength(10)]
        public string Body { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsAllowed { get; set; }
        public string? NotAllowedReason { get; set; }
        public AppUser AppUser { get; set; }
        public string AppUserId { get; set; }
        public string CoverImage { get; set; }
        public List<Like> Likes { get; set; }
        public List<Comment> Comments { get; set; }
    }
}

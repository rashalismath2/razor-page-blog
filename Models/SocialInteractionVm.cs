namespace BlogSite.Models
{
    public class SocialInteractionVm
    {
        public List<Comment> Comments { get; set; }
        public List<Like> Likes { get; set; }
        public bool IsLiked { get; set; }
        public bool IsSocialAllowed{ get; set; }
        public string AuthUserId { get; set; }
    }
}

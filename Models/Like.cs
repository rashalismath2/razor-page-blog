namespace BlogSite.Models
{
    public class Like
    {
        public int Id { get; set; }
        public AppUser LikedUser { get; set; }
    }
}

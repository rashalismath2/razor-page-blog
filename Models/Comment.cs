namespace BlogSite.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public AppUser CommentedUser { get; set; }
        public string Comments { get; set; }
        public DateTime Date { get; set; }
    }
}

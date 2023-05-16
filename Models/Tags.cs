namespace BlogSite.Models
{
    public class Tags
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public List<Post> Posts { get; set; }
    }
}

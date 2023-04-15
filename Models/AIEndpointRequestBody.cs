namespace BlogSite.Models
{
    public class AIEndpointRequestBody
    {
        public AIEndpointRequestBody(string title,string body)
        {
            Title = title;
            Body = body;
        }

        public string Title { get; }
        public string Body { get; }
    }
}

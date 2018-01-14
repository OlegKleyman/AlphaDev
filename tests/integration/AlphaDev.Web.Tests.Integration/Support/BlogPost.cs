namespace AlphaDev.Web.Tests.Integration.Support
{
    public class BlogPost
    {
        public string Title { get; }
        public string Content { get; }
        public string Dates { get; }

        public BlogPost(string title, string content, string dates)
        {
            Title = title;
            Content = content;
            Dates = dates;
        }
    }
}
namespace AlphaDev.Web.Tests.Integration.Support
{
    public class BlogPost
    {
        public BlogPost(string title, string content, BlogDate dates)
        {
            Title = title;
            Content = content;
            Dates = dates;
        }

        public string Title { get; }
        public string Content { get; }
        public BlogDate Dates { get; }
    }
}
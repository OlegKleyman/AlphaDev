namespace AlphaDev.Web.Tests.Integration.Support
{
    public class BlogPost
    {
        public NavigationLink NavigationLink { get; }

        public BlogPost(string title, string content, BlogDate dates)
        {
            Title = title;
            Content = content;
            Dates = dates;
        }

        public BlogPost(string title, string content, BlogDate dates, NavigationLink navigationLink) : this(title,content, dates)
        {
            NavigationLink = navigationLink;
        }

        public string Title { get; }
        public string Content { get; }
        public BlogDate Dates { get; }
    }
}
namespace AlphaDev.Core
{
    public class Blog : BlogBase
    {
        public Blog(string title, string content, Dates dates)
        {
            Title = title;
            Content = content;
            Dates = dates;
        }

        public override string Title { get; }

        public override Dates Dates { get; }

        public override string Content { get; }
    }
}
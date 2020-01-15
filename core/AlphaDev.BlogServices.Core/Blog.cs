namespace AlphaDev.BlogServices.Core
{
    public class Blog : BlogBase
    {
        public Blog(int id, string title, string content, in Dates dates)
        {
            Id = id;
            Title = title;
            Content = content;
            Dates = dates;
        }

        public Blog(string title, string content)
        {
            Title = title;
            Content = content;
        }

        public override string Title { get; }

        public override Dates Dates { get; }

        public override string Content { get; }

        public override int Id { get; }
    }
}
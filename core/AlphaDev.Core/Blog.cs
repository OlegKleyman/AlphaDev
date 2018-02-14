namespace AlphaDev.Core
{
    public class Blog : BlogBase
    {
        public Blog(int id, string title, string content, Dates dates)
        {
            Id = id;
            Title = title;
            Content = content;
            Dates = dates;
        }

        public override string Title { get; }

        public override Dates Dates { get; }

        public override string Content { get; }
        public override int Id { get; }
    }
}
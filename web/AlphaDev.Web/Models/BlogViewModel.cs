namespace AlphaDev.Web.Models
{
    public class BlogViewModel
    {
        public string Title { get; }

        public BlogViewModel(int id, string title, string content, DatesViewModel dates)
        {
            Id = id;
            Title = title;
            Content = content;
            Dates = dates;
        }

        public DatesViewModel Dates { get; }

        public string Content { get; }
        public int Id { get; set; }
    }
}
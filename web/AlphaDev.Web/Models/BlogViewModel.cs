namespace AlphaDev.Web.Models
{
    public class BlogViewModel
    {
        public BlogViewModel(string title, string content, DatesViewModel dates)
        {
            Title = title;
            Content = content;
            Dates = dates;
        }

        public string Title { get; set; }

        public DatesViewModel Dates { get; set; }

        public string Content { get; set; }
    }
}
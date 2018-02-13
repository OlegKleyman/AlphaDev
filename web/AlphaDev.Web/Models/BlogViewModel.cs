namespace AlphaDev.Web.Models
{
    public class BlogViewModel : PageViewModel
    {
        public BlogViewModel(string title, string content, DatesViewModel dates)
        {
            Title = title;
            Content = content;
            Dates = dates;
        }

        public DatesViewModel Dates { get; set; }

        public string Content { get; set; }
    }
}
namespace AlphaDev.Web.Models
{
    public class BlogViewModel : PageViewModel
    {
        public BlogViewModel(string title, string content, DatesViewModel dates) : base(title)
        {
            Content = content;
            Dates = dates;
        }

        public DatesViewModel Dates { get; }

        public string Content { get; }
    }
}
namespace AlphaDev.Web.Models
{
    public abstract class PageViewModel
    {
        protected PageViewModel(string title)
        {
            Title = title;
        }

        public string Title { get; }
    }
}
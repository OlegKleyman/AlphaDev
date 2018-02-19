using System;

namespace AlphaDev.Web.Models
{
    public class BlogViewModel
    {
        public BlogViewModel(int id, string title, string content, DatesViewModel dates)
        {
            Id = id;
            Title = title;
            Content = content;
            Dates = dates;
        }

        public string Title { get; }

        public DatesViewModel Dates { get; }

        public string Content { get; }
        public int Id { get; set; }

        public static BlogViewModel Welcome => WelcomeBlogViewModel.Value;

        private class WelcomeBlogViewModel:BlogViewModel
        {
            private static readonly Lazy<WelcomeBlogViewModel> Model = new Lazy<WelcomeBlogViewModel>(() => new WelcomeBlogViewModel());
            public static readonly WelcomeBlogViewModel Value = Model.Value;

            private WelcomeBlogViewModel() : base(default, "Welcome to my blog.",
                "```csharp\n" +
                "public void Main()\n" +
                "{\n" +
                "\t\tConsole.Writeline(\"Hello\");\n" +
                "}", default )
            {
            }
        }
    }
}
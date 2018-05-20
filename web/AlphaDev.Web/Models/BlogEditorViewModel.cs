using System.ComponentModel.DataAnnotations;
using Optional;

namespace AlphaDev.Web.Models
{
    public class BlogEditorViewModel
    {
        public BlogEditorViewModel(Option<DatesViewModel> dates, string content, string title)
        {
            Dates = dates;
            Content = content;
            Title = title;
        }

        [Required] public string Title { get; set; }

        [Required]
        [DataType(DataType.MultilineText)]
        public string Content { get; set; }

        public Option<DatesViewModel> Dates { get; set; }
    }
}
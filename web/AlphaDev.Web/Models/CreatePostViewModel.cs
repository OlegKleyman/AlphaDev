using System.ComponentModel.DataAnnotations;

namespace AlphaDev.Web.Models
{
    public class CreatePostViewModel
    {
        public CreatePostViewModel(string title, string content)
        {
            Title = title;
            Content = content;
        }

        [Required]
        public string Title { get; }

        [Required]
        [DataType(DataType.MultilineText)]
        public string Content { get; }
    }
}
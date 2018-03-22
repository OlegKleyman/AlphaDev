using System.ComponentModel.DataAnnotations;
using AlphaDev.Web.Support;
using Microsoft.AspNetCore.Mvc;

namespace AlphaDev.Web.Models
{
    [ModelBinder(typeof(CreatePostModelBinder))]
    public class CreatePostViewModel
    {
        public CreatePostViewModel(string title, string content)
        {
            Title = title;
            Content = content;
        }

        [Required] public string Title { get; }

        [Required]
        [DataType(DataType.MultilineText)]
        public string Content { get; }
    }
}
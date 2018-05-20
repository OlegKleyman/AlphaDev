using System.ComponentModel.DataAnnotations;
using AlphaDev.Web.Support;
using Microsoft.AspNetCore.Mvc;

namespace AlphaDev.Web.Models
{
    [ModelBinder(typeof(EditPostModelBinder))]
    public class EditPostViewModel
    {
        public EditPostViewModel(string title, string content, DatesViewModel dates)
        {
            Title = title;
            Content = content;
            Dates = dates;
        }

        public DatesViewModel Dates { get; }

        [Required] public string Title { get; }

        [Required]
        [DataType(DataType.MultilineText)]
        public string Content { get; }
    }
}
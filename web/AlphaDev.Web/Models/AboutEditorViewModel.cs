using System.ComponentModel.DataAnnotations;
using AlphaDev.Web.Support;
using Microsoft.AspNetCore.Mvc;

namespace AlphaDev.Web.Models
{
    [ModelBinder(typeof(AboutEditModelBinder))]
    public class AboutEditorViewModel
    {
        public AboutEditorViewModel(string value)
        {
            Value = value;
        }

        public AboutEditorViewModel()
        {
        }

        [Required]
        public string Value { get; set; }
    }
}
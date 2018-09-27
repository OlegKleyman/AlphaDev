using System.ComponentModel.DataAnnotations;
using AlphaDev.Web.Support;
using Microsoft.AspNetCore.Mvc;

namespace AlphaDev.Web.Models
{
    [ModelBinder(typeof(AboutCreateModelBinder))]
    public class AboutCreateViewModel
    {
        public AboutCreateViewModel()
        {
        }

        public AboutCreateViewModel(string value)
        {
            Value = value;
        }

        [Required] public string Value { get; set; }
    }
}
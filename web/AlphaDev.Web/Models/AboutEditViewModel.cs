using System.ComponentModel.DataAnnotations;
using AlphaDev.Web.Support;
using Microsoft.AspNetCore.Mvc;

namespace AlphaDev.Web.Models
{
    [ModelBinder(typeof(AboutEditModelBinder))]
    public class AboutEditViewModel
    {
        public AboutEditViewModel(string value)
        {
            Value = value;
        }

        public AboutEditViewModel()
        {
        }

        [Required] public string Value { get; set; }
    }
}
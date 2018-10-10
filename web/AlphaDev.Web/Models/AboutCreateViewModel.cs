using System.ComponentModel.DataAnnotations;
using AlphaDev.Web.Support;
using Microsoft.AspNetCore.Mvc;

namespace AlphaDev.Web.Models
{
    [ModelBinder(typeof(AboutCreateModelBinder))]
    public class AboutCreateViewModel : SingleValueViewModel
    {
        public AboutCreateViewModel() : base(string.Empty)
        {
        }

        public AboutCreateViewModel(string value) : base(value)
        {
        }
    }
}
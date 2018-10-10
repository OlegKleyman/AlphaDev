using AlphaDev.Web.Support;
using Microsoft.AspNetCore.Mvc;

namespace AlphaDev.Web.Models
{
    [ModelBinder(typeof(AboutEditModelBinder))]
    public class AboutEditViewModel : SingleValueViewModel
    {
        public AboutEditViewModel(string value) : base(value)
        {
        }

        public AboutEditViewModel() : base(string.Empty)
        {
        }
    }
}
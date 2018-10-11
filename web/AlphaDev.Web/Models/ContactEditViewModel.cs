using AlphaDev.Web.Support;
using Microsoft.AspNetCore.Mvc;

namespace AlphaDev.Web.Models
{
    [ModelBinder(typeof(ContactEditModelBinder))]
    public class ContactEditViewModel : SingleValueViewModel
    {
        public ContactEditViewModel(string value) : base(value)
        {
        }

        public ContactEditViewModel() : base(string.Empty)
        {
        }
    }
}
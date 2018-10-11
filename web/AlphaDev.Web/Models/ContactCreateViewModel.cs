using AlphaDev.Web.Support;
using Microsoft.AspNetCore.Mvc;

namespace AlphaDev.Web.Models
{
    [ModelBinder(typeof(ContactCreateModelBinder))]
    public class ContactCreateViewModel : SingleValueViewModel
    {
        public ContactCreateViewModel(string value) : base(value)
        {
        }

        public ContactCreateViewModel() : base(string.Empty)
        {
        }
    }
}
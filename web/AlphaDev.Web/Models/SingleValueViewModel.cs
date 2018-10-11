using System.ComponentModel.DataAnnotations;

namespace AlphaDev.Web.Models
{
    public abstract class SingleValueViewModel
    {
        protected SingleValueViewModel(string value)
        {
            Value = value;
        }

        [Required]
        public string Value { get; set; }
    }
}
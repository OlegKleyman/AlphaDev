using System.ComponentModel.DataAnnotations;

namespace AlphaDev.Web.Models
{
    public class SimpleEditorViewModel
    {
        [Required]
        public string Value { get; set; }
    }
}
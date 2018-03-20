using System.ComponentModel.DataAnnotations;

namespace AlphaDev.Web.Models
{
    public class CreatePost
    {
        [Required]
        public string Title { get; set; }

        [Required]
        [DataType(DataType.MultilineText)]
        public string Content { get; set; }
    }
}
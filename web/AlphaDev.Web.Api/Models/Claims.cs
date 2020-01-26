using System.ComponentModel.DataAnnotations;

namespace AlphaDev.Web.Api.Models
{
    public class Claims
    {
        [Required]
        public string? Username { get; set; }

        [Required]
        public string? Password { get; set; }
    }
}
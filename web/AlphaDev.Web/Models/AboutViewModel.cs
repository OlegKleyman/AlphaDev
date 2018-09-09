using JetBrains.Annotations;

namespace AlphaDev.Web.Models
{
    public class AboutViewModel
    {
        public AboutViewModel([NotNull] string details)
        {
            Details = details;
        }

        public string Details { get; }
    }
}
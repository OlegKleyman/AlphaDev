namespace AlphaDev.Web.Tests.Integration.Support
{
    public class NavigationLink
    {
        public NavigationLink(string href) => Href = href;

        public string Href { get; }
    }
}
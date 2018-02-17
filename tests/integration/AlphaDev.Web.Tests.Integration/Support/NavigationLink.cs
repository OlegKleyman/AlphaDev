using System;

namespace AlphaDev.Web.Tests.Integration.Support
{
    public class NavigationLink
    {
        public string Href { get; }

        public NavigationLink(string href)
        {
            Href = href;
        }
    }
}
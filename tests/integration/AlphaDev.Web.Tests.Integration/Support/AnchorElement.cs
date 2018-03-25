using OpenQA.Selenium;

namespace AlphaDev.Web.Tests.Integration.Support
{
    public class AnchorElement : NavigationElement
    {
        public AnchorElement(IWebElement element) : base(element)
        {
            Href = element.GetAttribute("href");
        }

        public string Href { get; }
    }
}
using OpenQA.Selenium;

namespace AlphaDev.Web.Tests.Integration.Support
{
    public class ButtonElement : NavigationElement
    {
        public ButtonElement(IWebElement element) : base(element)
        {
        }
    }
}
using System;
using OpenQA.Selenium;

namespace AlphaDev.Web.Tests.Integration.Support
{
    public class NavigationElement
    {
        public NavigationElement(IWebElement element)
        {
            Text = element.Text;
            Click = element.Click;
        }

        public Action Click { get; }

        public string Text { get; }

        public static NavigationElement FromWebElement(IWebElement element)
        {
            NavigationElement realizedElement;

            switch (element.TagName)
            {
                case "a":
                    realizedElement = new AnchorElement(element);
                    break;
                case "button":
                    realizedElement = new ButtonElement(element);
                    break;
                default:
                    realizedElement = new NavigationElement(element);
                    break;
            }

            return realizedElement;
        }
    }
}
using System;
using AlphaDev.Web.Tests.Integration.Extensions;
using OpenQA.Selenium;

namespace AlphaDev.Web.Tests.Integration.Support
{
    public abstract class DetailsWebPage : WebPage
    {
        protected DetailsWebPage(IWebDriver driver, Uri baseUrl) : base(driver, baseUrl)
        {
            Create = new SingleValueCreateEditorWebPage(driver, baseUrl);
            Edit = new SingleValueEditEditorWebPage(driver, BaseUrl);
        }

        public string Details => Driver.FindElement(By.CssSelector(".bubble div:nth-child(2)")).GetInnerHtml().Trim();

        public SingleValueCreateEditorWebPage Create { get; }

        public SingleValueEditEditorWebPage Edit { get; }

        public abstract void EditValue();
    }
}
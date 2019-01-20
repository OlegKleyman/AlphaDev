using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using AlphaDev.Web.Tests.Integration.Extensions;
using JetBrains.Annotations;
using OpenQA.Selenium;
using Optional;

namespace AlphaDev.Web.Tests.Integration.Support
{
    public class PostsWebPage : WebPage
    {
        public Page CurrentPage
        {
            get
            {
                var displayValue = new Uri(Driver.Url).Segments.Last().TrimEnd('/');
                var number = int.Parse(displayValue);
                return new Page(new PageIdentity(displayValue, number), DisplayFormat.Number, new PageAttributes());
            }
        }

        public PostsWebPage(IWebDriver driver, Uri baseUrl) : base(driver, new Uri(baseUrl, "page/1/"))
        {
            Create = new BlogEditorWebPage(Driver, new Uri(baseUrl, "create"));
            Edit = new BlogEditorWebPage(Driver, new Uri(baseUrl, "edit"));
            PostBaseUrl = baseUrl;
        }

        [NotNull]
        public IEnumerable<BlogPost> Posts
        {
            get
            {
                return Driver.FindElements(By.CssSelector(".blog .bubble")).Select(element =>
                    new BlogPost(element.FindElement(By.CssSelector(".title h3")).Text,
                        element.FindElement(By.CssSelector(".bubble-content")).GetAttribute("innerHTML").Trim(),
                        new BlogDate(element.FindElement(By.CssSelector(".created-date")).Text,
                            (element.FindElements(By.CssSelector(".modified-date")).FirstOrDefault()?.Text)
                            .SomeNotNull()),
                        new NavigationLink(element.FindElement(By.CssSelector(".navigation-link a"))
                            .GetAttribute("href"))));
            }
        }

        [NotNull]
        public BlogPost Post => new BlogPost(Driver.FindElement(By.CssSelector(".blog .title h3")).Text,
            Driver.FindElement(By.CssSelector(".blog .bubble-content")).GetAttribute("innerHTML").Trim(),
            new BlogDate(Driver.FindElement(By.CssSelector(".blog .created-date")).Text,
                (Driver.FindElements(By.CssSelector(".blog .modified-date")).FirstOrDefault()?.Text)
                .SomeNotNull()));

        public BlogEditorWebPage Create { get; }

        public BlogEditorWebPage Edit { get; }

        [NotNull]
        public IEnumerable<PostsWebPageLink> Pages
        {
            get
            {
                return Driver.FindElements(By.CssSelector(".pages .page"))
                    .Select(x =>
                    {
                        var isActive = !x.TagName.Equals("a", StringComparison.OrdinalIgnoreCase);
                        int pageNumber;
                        var displayFormat = x.Text.IsEllipses() ? DisplayFormat.Text : DisplayFormat.Number; ;

                        if (isActive)
                        {
                            if (!int.TryParse(x.Text, NumberStyles.Integer, CultureInfo.InvariantCulture,
                                out pageNumber))
                            {
                                throw new InvalidCastException($"Page text \"{x.Text}\" does not contain number.");
                            }
                        }
                        else
                        {
                            var href = new Uri(x.GetAttribute("href"));
                            var urlPageNumber = href.Segments.Last();

                            if (!int.TryParse(urlPageNumber, NumberStyles.Integer, CultureInfo.InvariantCulture,
                                out pageNumber))
                            {
                                throw new InvalidCastException($"Page text \"{urlPageNumber}\" does not contain number.");
                            }
                        }

                        var pageAttributes = isActive ? new PageAttributes() : new PageAttributes(BaseUrl, pageNumber);
                        return new PostsWebPageLink(Driver,
                            new Page(new PageIdentity(x.Text, pageNumber), displayFormat, pageAttributes));
                    });
            }
        }

        public Uri PostBaseUrl { get; }

        public override WebPage GoTo(int id)
        {
            Driver.Navigate().GoToUrl($"{PostBaseUrl.AbsoluteUri.Trim('/')}/{id}");
            return this;
        }
    }
}
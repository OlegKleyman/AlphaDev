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
        public Page CurrentPage { get; }

        public PostsWebPage(IWebDriver driver, Uri baseUrl) : this(driver, baseUrl, new Page(new PageIdentity("1", 1), DisplayFormat.Number, true))
        {
            Create = new BlogEditorWebPage(Driver, new Uri(BaseUrl, "create"));
            Edit = new BlogEditorWebPage(Driver, new Uri(BaseUrl, "edit"));
        }

        public PostsWebPage(IWebDriver driver, Uri baseUrl, Page currentPage) : base(driver, baseUrl)
        {
            CurrentPage = currentPage;
            Create = new BlogEditorWebPage(Driver, new Uri(BaseUrl, "create"));
            Edit = new BlogEditorWebPage(Driver, new Uri(BaseUrl, "edit"));
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

                        return new PostsWebPageLink(Driver, BaseUrl,
                            new Page(new PageIdentity(x.Text, pageNumber), displayFormat, isActive));
                    });
            }
        }
    }
}
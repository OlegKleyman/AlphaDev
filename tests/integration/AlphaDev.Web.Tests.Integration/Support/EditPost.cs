using System;
using System.Collections.Generic;
using System.Globalization;
using OpenQA.Selenium;

namespace AlphaDev.Web.Tests.Integration.Support
{
    public class EditPost
    {
        private readonly Dictionary<int, EditWebPage> _pageCache;
        private readonly IWebDriver _driver;
        private readonly Uri _baseUrl;

        public EditPost(IWebDriver driver, Uri baseUrl)
        {
            _driver = driver;
            _baseUrl = baseUrl;
            _pageCache = new Dictionary<int, EditWebPage>();
        }

        public EditWebPage this[int id]
        {
            get
            {
                if (!_pageCache.TryGetValue(id, out EditWebPage v))
                {
                    v = new EditWebPage(_driver, new Uri(_baseUrl, id.ToString(CultureInfo.InvariantCulture)));
                    _pageCache.Add(id, v);
                }

                return v;
            }
        }
    }
}
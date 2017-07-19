using System;
using System.Collections.Generic;
using System.Text;

namespace AlphaDev.Core.Tests.Unit
{
    using AppDev.Core;

    using FluentAssertions;

    using Optional;

    using Xunit;

    public class BlogTests
    {
        [Fact]
        public void ConstructorShouldSetProperties()
        {
            var dates = new Dates(default(DateTime), Option.None<DateTime>());

            var title = "test title";
            var content = "test content";

            var blog = new Blog(title, content, dates);

            blog.Dates.ShouldBeEquivalentTo(dates);
            blog.Title.ShouldBeEquivalentTo(title);
            blog.Content.ShouldBeEquivalentTo(content);
        }
    }
}

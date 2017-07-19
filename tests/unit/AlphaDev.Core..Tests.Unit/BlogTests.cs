using System;
using AppDev.Core;
using FluentAssertions;
using Optional;
using Xunit;

namespace AlphaDev.Core.Tests.Unit
{
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
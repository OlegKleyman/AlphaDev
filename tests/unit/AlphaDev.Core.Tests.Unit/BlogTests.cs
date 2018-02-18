using System;
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
            var dates = new Dates(default, Option.None<DateTime>());

            var title = "test title";
            var content = "test content";
            var id = 123;

            var blog = new Blog(id, title, content, dates);

            blog.Id.Should().Be(id);
            blog.Dates.Should().BeEquivalentTo(dates);
            blog.Title.Should().BeEquivalentTo(title);
            blog.Content.Should().BeEquivalentTo(content);
        }
    }
}
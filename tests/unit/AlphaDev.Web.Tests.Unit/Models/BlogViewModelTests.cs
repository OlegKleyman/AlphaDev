namespace AlphaDev.Web.Tests.Unit.Models
{
    using System;

    using AlphaDev.Web.Models;

    using FluentAssertions;

    using Optional;

    using Xunit;

    public class BlogViewModelTests
    {
        [Fact]
        public void ConstructorShouldSetProperties()
        {
            var dates = new DatesViewModel(default(DateTime), Option.None<DateTime>());

            var title = "test title";
            var content = "test content";

            var blog = new BlogViewModel(title, content, dates);

            blog.Dates.ShouldBeEquivalentTo(dates);
            blog.Title.ShouldBeEquivalentTo(title);
            blog.Content.ShouldBeEquivalentTo(content);
        }
    }
}

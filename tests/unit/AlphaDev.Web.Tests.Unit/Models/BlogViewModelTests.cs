using System;
using AlphaDev.Web.Models;
using FluentAssertions;
using Optional;
using Xunit;

namespace AlphaDev.Web.Tests.Unit.Models
{
    public class BlogViewModelTests
    {
        [Fact]
        public void ConstructorShouldSetProperties()
        {
            var dates = new DatesViewModel(default(DateTime), Option.None<DateTime>());

            var title = "test title";
            var content = "test content";
            var id = 123;

            var blog = new BlogViewModel(id, title, content, dates);

            blog.Id.ShouldBeEquivalentTo(id);
            blog.Dates.ShouldBeEquivalentTo(dates);
            blog.Title.ShouldBeEquivalentTo(title);
            blog.Content.ShouldBeEquivalentTo(content);
        }
    }
}
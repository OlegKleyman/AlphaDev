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
            var dates = new DatesViewModel(default, Option.None<DateTime>());

            var title = "test title";
            var content = "test content";
            var id = 123;

            var blog = new BlogViewModel(id, title, content, dates);

            blog.Id.Should().Be(id);
            blog.Dates.Should().BeEquivalentTo(dates);
            blog.Title.Should().BeEquivalentTo(title);
            blog.Content.Should().BeEquivalentTo(content);
        }

        [Fact]
        public void WelcomeShouldReturnDefaultWelcomeBlogModelValues()
        {
            const string content = "```csharp\n" +
                          "public void Main()\n" +
                          "{\n" +
                          "\t\tConsole.Writeline(\"Hello\");\n" +
                          "}";

            BlogViewModel.Welcome.Should().BeEquivalentTo(new
            {
                Id = default(int),
                Title = "Welcome to my blog.",
                Content = content,
                Dates = default(DatesViewModel)
            });
        }
    }
}
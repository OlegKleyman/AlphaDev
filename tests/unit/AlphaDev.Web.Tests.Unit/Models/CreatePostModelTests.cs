using AlphaDev.Web.Models;
using FluentAssertions;
using Xunit;

namespace AlphaDev.Web.Tests.Unit.Models
{
    public class CreatePostModelTests
    {
        [Fact]
        public void ConstructorShouldSetProperties()
        {
            const string title = "title";
            const string content = "content";

            var model = new CreatePostViewModel(title, content);

            model.Should().BeEquivalentTo(new {Title = title, Content = content});
        }
    }
}
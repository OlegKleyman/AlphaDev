using AlphaDev.Web.Models;
using FluentAssertions;
using Xunit;

namespace AlphaDev.Web.Tests.Unit.Models
{
    public class ErrorModelTests
    {
        [Fact]
        public void ConstructorShouldSetProperties()
        {
            new ErrorModel(200, "test").Should().BeEquivalentTo(new {Status = 200, Message = "test"});

        }
    }
}
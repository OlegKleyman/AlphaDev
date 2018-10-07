using AlphaDev.Web.Models;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace AlphaDev.Web.Tests.Unit.Models
{
    public class SingleValueEditViewModelTests
    {
        [Fact]
        public void ConstructorShouldInitializeSingleValueEditViewModelWithValue()
        {
            var model = Substitute.For<SingleValueEditViewModel>("test");
            model.Value.Should().BeEquivalentTo("test");
        }

        [Fact]
        public void ValueShouldGetAndSet()
        {
            var model = Substitute.For<SingleValueEditViewModel>(default(string));
            model.Value = "test";
            model.Value.Should().BeEquivalentTo("test");
        }
    }
}

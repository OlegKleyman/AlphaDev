using AlphaDev.Web.Models;
using FluentAssertions;
using Xunit;

namespace AlphaDev.Web.Tests.Unit.Models
{
    public class ContactCreateViewModelTests
    {
        [Fact]
        public void ConstructorShouldInitializeWithArguments()
        {
            const string value = "value";
            new ContactCreateViewModel(value).Should().BeEquivalentTo(new { Value = value });
        }

        [Fact]
        public void DefaultConstructorShouldInitializeWithEmptyString()
        {
            new ContactCreateViewModel().Value.Should().BeEmpty();
        }
    }
}
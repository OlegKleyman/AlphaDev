using System;
using AlphaDev.Web.Models;
using FluentAssertions;
using Xunit;

namespace AlphaDev.Web.Tests.Unit.Models
{
    public class AboutCreateViewModelTests
    {
        [Fact]
        public void ConstructorShouldInitializeWithArguments()
        {
            const string value = "value";
            new AboutCreateViewModel(value).Should().BeEquivalentTo(new { Value = value });
        }

        [Fact]
        public void DefaultConstructorShouldInitializeWithEmptyString()
        {
            new AboutCreateViewModel().Value.Should().BeEmpty();
        }
    }
}
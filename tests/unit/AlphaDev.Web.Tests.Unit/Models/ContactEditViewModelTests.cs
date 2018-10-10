using System;
using AlphaDev.Web.Models;
using FluentAssertions;
using Xunit;

namespace AlphaDev.Web.Tests.Unit.Models
{
    public class ContactEditViewModelTests
    {
        [Fact]
        public void ConstructorShouldInitializeWithArguments()
        {
            const string value = "value";
            new ContactEditViewModel(value).Should().BeEquivalentTo(new { Value = value });
        }

        [Fact]
        public void DefaultConstructorShouldInitializeWithEmptyString()
        {
            new ContactEditViewModel().Value.Should().BeEmpty();
        }
    }
}
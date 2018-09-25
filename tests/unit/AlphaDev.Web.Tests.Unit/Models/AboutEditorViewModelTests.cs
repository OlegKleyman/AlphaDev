using System;
using AlphaDev.Web.Models;
using FluentAssertions;
using Xunit;

namespace AlphaDev.Web.Tests.Unit.Models
{
    public class AboutEditViewModelTests
    {
        [Fact]
        public void ConstructorShouldInitializeAboutEditorViewModelWithArguments()
        {
            const string value = "value";
            new AboutEditViewModel(value).Should().BeEquivalentTo(new {Value = value});
        }

        [Fact]
        public void DefaultConstructorShouldInitializeAboutEditorViewModel()
        {
            Action constructor = () => new AboutEditViewModel();
            constructor.Should().NotThrow();
        }
    }
}

using System;
using AlphaDev.Web.Models;
using FluentAssertions;
using Xunit;

namespace AlphaDev.Web.Tests.Unit.Models
{
    public class AboutEditorViewModelTests
    {
        [Fact]
        public void ConstructorShouldInitializeAboutEditorViewModelWithArguments()
        {
            const string value = "value";
            new AboutEditorViewModel(value).Should().BeEquivalentTo(new {Value = value});
        }

        [Fact]
        public void DefaultConstructorShouldInitializeAboutEditorViewModel()
        {
            Action constructor = () => new AboutEditorViewModel();
            constructor.Should().NotThrow();
        }
    }
}

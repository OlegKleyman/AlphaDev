using AlphaDev.Web.Models;
using FluentAssertions;
using JetBrains.Annotations;
using Xunit;

namespace AlphaDev.Web.Tests.Unit.Models
{
    public class SimpleEditorViewModelTests
    {
        [NotNull]
        private SimpleEditorViewModel GetSimpleEditorViewModel()
        {
            return new SimpleEditorViewModel();
        }

        [Fact]
        public void ValueShouldGetAndSetValue()
        {
            var model = GetSimpleEditorViewModel();
            model.Value = "value";
            model.Value.Should().BeEquivalentTo("value");
        }
    }
}
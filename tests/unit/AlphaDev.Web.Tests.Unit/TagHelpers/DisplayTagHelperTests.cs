using System;
using System.IO;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using AlphaDev.Web.TagHelpers;
using FluentAssertions;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Xunit;

namespace AlphaDev.Web.Tests.Unit.TagHelpers
{
    public class DisplayTagHelperTests
    {
        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void ProcessShouldDisplayContentsBasedOnValue(bool value)
        {
            var sut = GetDisplayTagHelper();

            sut.Value = value;
            var tagHelperOutput = new TagHelperOutput(default, new TagHelperAttributeList(), (_, __) => null);
            tagHelperOutput.Content.Append("test");
            sut.Process(null, tagHelperOutput);

            var writer = new StringWriter();

            tagHelperOutput.Content.WriteTo(writer, HtmlEncoder.Default);

            writer.ToString().Should().BeEquivalentTo(value ? "test" : string.Empty);
        }

        [NotNull]
        private DisplayTagHelper GetDisplayTagHelper()
        {
            return new DisplayTagHelper();
        }

        [Fact]
        public void ProcessAsyncShouldThrowArgumentNullExceptionWhenOutputArgumentIsNull()
        {
            var sut = GetDisplayTagHelper();

            async Task ProcessAsync()
            {
                await sut.ProcessAsync(null, null);
            }

            new Func<Task>(ProcessAsync).Should().Throw<ArgumentNullException>()
                .WithMessage("Value cannot be null.\r\nParameter name: output");
        }

        [Fact]
        public void ProcessShouldSetValuePropertyToTrueByDefault()
        {
            GetDisplayTagHelper().Value
                .Should().Be(true);
        }

        [Fact]
        public void ProcessShouldThrowArgumentNullExceptionWhenOutputArgumentIsNull()
        {
            var sut = GetDisplayTagHelper();

            Action process = () => sut.Process(null, null);

            process.Should().Throw<ArgumentNullException>()
                .WithMessage("Value cannot be null.\r\nParameter name: output")
                .Which.ParamName.Should().BeEquivalentTo("output");
        }
    }
}
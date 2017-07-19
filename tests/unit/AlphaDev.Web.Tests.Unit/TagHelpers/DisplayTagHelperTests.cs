using System;
using System.IO;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using AlphaDev.Web.TagHelpers;
using FluentAssertions;
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
            var tagHelperOutput = new TagHelperOutput(default(string), new TagHelperAttributeList(), (_, __) => null);
            tagHelperOutput.Content.Append("test");
            sut.Process(null, tagHelperOutput);

            var writer = new StringWriter();

            tagHelperOutput.Content.WriteTo(writer, HtmlEncoder.Default);

            writer.ToString().ShouldBeEquivalentTo(value ? "test" : string.Empty);
        }

        private DisplayTagHelper GetDisplayTagHelper()
        {
            return new DisplayTagHelper();
        }

        [Fact]
        public void ProcessAsyncShouldThrowArgumentNullExceptionWhenOutputArgumentIsNull()
        {
            var sut = GetDisplayTagHelper();

            Func<Task> process = async () => await sut.ProcessAsync(null, null);

            Action asserter = () => process.ShouldThrow<ArgumentNullException>()
                .WithMessage("Value cannot be null.\r\nParameter name: output");

            asserter.ShouldNotThrow();
        }

        [Fact]
        public void ProcessShouldSetValuePropertyToTrueByDefault()
        {
            GetDisplayTagHelper().Value
                .ShouldBeEquivalentTo(true);
        }

        [Fact]
        public void ProcessShouldThrowArgumentNullExceptionWhenOutputArgumentIsNull()
        {
            var sut = GetDisplayTagHelper();

            Action process = () => sut.Process(null, null);

            process.ShouldThrow<ArgumentNullException>().WithMessage("Value cannot be null.\r\nParameter name: output")
                .Which.ParamName.ShouldBeEquivalentTo("output");
        }
    }
}
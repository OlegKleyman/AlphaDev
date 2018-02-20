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
    public class MarkdownTagHelperTests
    {
        [Theory]
        [InlineData("**test**", "<p><strong>test</strong></p>\n")]
        [InlineData("`test`", "<p><code>test</code></p>\n")]
        [InlineData("`<s>test</s>`", "<p><code>&lt;s&gt;test&lt;/s&gt;</code></p>\n")]
        [InlineData("\n   `<s>test</s>`", "<p><code>&lt;s&gt;test&lt;/s&gt;</code></p>\n")]
        [InlineData("```csharp\n<s>test</s>\n```",
            "<pre><code class=\"language-csharp\">&lt;s&gt;test&lt;/s&gt;\n</code></pre>\n")]
        public void ProcessShouldConvertMarkdownToHtml(string markdown, string expected)
        {
            var sut = GetMarkdownTagHelper();

            var tagHelperOutput = new TagHelperOutput(default, new TagHelperAttributeList(), (_, __) =>
            {
                var content = new DefaultTagHelperContent();

                content.SetHtmlContent(markdown);
                return Task.FromResult<TagHelperContent>(content);
            });

            sut.Process(null, tagHelperOutput);

            var writer = new StringWriter();

            tagHelperOutput.Content.WriteTo(writer, HtmlEncoder.Default);

            writer.ToString().Should().BeEquivalentTo(expected);
        }

        [Theory]
        [InlineData("**test**", "<p><strong>test</strong></p>\n")]
        [InlineData("`test`", "<p><code>test</code></p>\n")]
        [InlineData("`<s>test</s>`", "<p><code>&lt;s&gt;test&lt;/s&gt;</code></p>\n")]
        [InlineData("\n   `<s>test</s>`", "<p><code>&lt;s&gt;test&lt;/s&gt;</code></p>\n")]
        [InlineData("```csharp\n<s>test</s>\n```",
            "<pre><code class=\"language-csharp\">&lt;s&gt;test&lt;/s&gt;\n</code></pre>\n")]
        public async Task ProcessAsyncShouldConvertMarkdownToHtml(string markdown, string expected)
        {
            var sut = GetMarkdownTagHelper();

            var tagHelperOutput = new TagHelperOutput(default, new TagHelperAttributeList(), (_, __) =>
            {
                var content = new DefaultTagHelperContent();

                content.SetHtmlContent(markdown);
                return Task.FromResult<TagHelperContent>(content);
            });

            await sut.ProcessAsync(null, tagHelperOutput);

            var writer = new StringWriter();

            tagHelperOutput.Content.WriteTo(writer, HtmlEncoder.Default);

            writer.ToString().Should().BeEquivalentTo(expected);
        }

        private MarkdownTagHelper GetMarkdownTagHelper()
        {
            return new MarkdownTagHelper();
        }

        [Fact]
        public void ProcessShouldSetTagNameToEmptyString()
        {
            var sut = GetMarkdownTagHelper();

            var tagHelperOutput = new TagHelperOutput(default, new TagHelperAttributeList(), (_, __) =>
            {
                var content = new DefaultTagHelperContent();

                content.SetHtmlContent(string.Empty);
                return Task.FromResult<TagHelperContent>(content);
            }) {TagName = "test"};


            sut.Process(null, tagHelperOutput);

            tagHelperOutput.TagName.Should().BeEmpty();
        }

        [Fact]
        public void ProcessShouldThrowArgumentNullExceptionWhenOutputArgumentIsNull()
        {
            var sut = GetMarkdownTagHelper();

            Action process = () => sut.Process(null, null);

            process.Should().Throw<ArgumentNullException>()
                .WithMessage("Value cannot be null.\r\nParameter name: output")
                .Which.ParamName.Should().BeEquivalentTo("output");
        }
    }
}
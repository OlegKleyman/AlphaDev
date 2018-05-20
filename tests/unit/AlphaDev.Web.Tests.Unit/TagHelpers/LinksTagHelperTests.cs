using System.Collections.Generic;
using System.IO;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using AlphaDev.Web.TagHelpers;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Xunit;

namespace AlphaDev.Web.Tests.Unit.TagHelpers
{
    public class LinksTagHelperTests
    {
        private LinksTagHelper GetLinksTagHelper()
        {
            return new LinksTagHelper();
        }

        [Fact]
        public void ContextShouldGetAndSetViewContext()
        {
            var helper = GetLinksTagHelper();

            var context = new ViewContext();
            helper.Context = context;
            helper.Context.Should().Be(context);
        }

        [Fact]
        public void ProcessShouldSetContentToInlineStylesFromHashSet()
        {
            var helper = GetLinksTagHelper();

            var tagHelperOutput = new TagHelperOutput(default, new TagHelperAttributeList(),
                (_, __) => Task.FromResult<TagHelperContent>(new DefaultTagHelperContent()));

            helper.Context = new ViewContext
            {
                ViewData = {["InlineStyles"] = new HashSet<string>(new[] {"test"})}
            };

            helper.Process(null, tagHelperOutput);

            var writer = new StringWriter();

            tagHelperOutput.Content.WriteTo(writer, HtmlEncoder.Default);

            writer.ToString().Should().BeEquivalentTo("<style>test</style>");
        }

        [Fact]
        public void ProcessShouldSetContentToLinkTagsFromHashSet()
        {
            var helper = GetLinksTagHelper();

            var tagHelperOutput = new TagHelperOutput(default, new TagHelperAttributeList(),
                (_, __) => Task.FromResult<TagHelperContent>(new DefaultTagHelperContent()));

            helper.Context = new ViewContext
            {
                ViewData = {["AllLinks"] = new HashSet<string>(new[] {"test"})}
            };

            helper.Process(null, tagHelperOutput);

            var writer = new StringWriter();

            tagHelperOutput.Content.WriteTo(writer, HtmlEncoder.Default);

            writer.ToString().Should().BeEquivalentTo("<link href=\"test\" rel=\"stylesheet\" />");
        }
    }
}
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
    public class ScriptsTagHelperTests
    {
        private ScriptsTagHelper GetScriptsTagHelper()
        {
            return new ScriptsTagHelper();
        }

        [Fact]
        public void ContextShouldGetAndSetViewContext()
        {
            var helper = GetScriptsTagHelper();

            var context = new ViewContext();
            helper.Context = context;
            helper.Context.Should().Be(context);
        }

        [Fact]
        public void ProcessShouldSetContentToInlineScriptsFromString()
        {
            var helper = GetScriptsTagHelper();

            var tagHelperOutput = new TagHelperOutput(default, new TagHelperAttributeList(),
                (_, __) => Task.FromResult<TagHelperContent>(new DefaultTagHelperContent()));

            helper.Context = new ViewContext
            {
                ViewData =
                {
                    ["InlineScripts"] = "test"
                }
            };

            helper.Process(null, tagHelperOutput);

            var writer = new StringWriter();

            tagHelperOutput.Content.WriteTo(writer, HtmlEncoder.Default);

            writer.ToString().Should().BeEquivalentTo("test");
        }

        [Fact]
        public void ProcessShouldSetContentToScriptTagsFromHashSet()
        {
            var helper = GetScriptsTagHelper();

            var tagHelperOutput = new TagHelperOutput(default, new TagHelperAttributeList(),
                (_, __) => Task.FromResult<TagHelperContent>(new DefaultTagHelperContent()));

            helper.Context = new ViewContext
            {
                ViewData = {["AllScripts"] = new HashSet<string>(new[] {"test"})}
            };

            helper.Process(null, tagHelperOutput);

            var writer = new StringWriter();

            tagHelperOutput.Content.WriteTo(writer, HtmlEncoder.Default);

            writer.ToString().Should().BeEquivalentTo("<script src=\"test\"></script>");
        }
    }
}
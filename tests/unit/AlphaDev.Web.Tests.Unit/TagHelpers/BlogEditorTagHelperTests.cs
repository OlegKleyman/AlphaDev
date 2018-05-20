using System.Collections.Generic;
using System.IO;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using AlphaDev.Core;
using AlphaDev.Web.TagHelpers;
using FluentAssertions;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using NSubstitute;
using Xunit;

namespace AlphaDev.Web.Tests.Unit.TagHelpers
{
    public class BlogEditorTagHelperTests
    {
        private BlogEditorTagHelper GetBlogEditorTagHelper(IHtmlHelper htmlHelper, IUrlHelperFactory urlHelperFactory,
            IPrefixGenerator prefixGenerator)
        {
            return new BlogEditorTagHelper(htmlHelper, urlHelperFactory, prefixGenerator);
        }

        private BlogEditorTagHelper GetBlogEditorTagHelper(IHtmlHelper htmlHelper, IUrlHelperFactory urlHelperFactory)
        {
            return GetBlogEditorTagHelper(htmlHelper, urlHelperFactory, Substitute.For<IPrefixGenerator>());
        }

        [Fact]
        public void ContextShouldGetAndSetViewContext()
        {
            var helper = GetBlogEditorTagHelper(default, default);

            var context = new ViewContext();
            helper.Context = context;
            helper.Context.Should().Be(context);
        }

        [Fact]
        public void ProcessShouldSetAllLinksWithDependentLinkUrls()
        {
            var tagHelperOutput = new TagHelperOutput(default, new TagHelperAttributeList(),
                (_, __) => Task.FromResult<TagHelperContent>(new DefaultTagHelperContent()));

            var urlHelperFactory = Substitute.For<IUrlHelperFactory>();
            var urlHelper = Substitute.For<IUrlHelper>();

            var helper = GetBlogEditorTagHelper(Substitute.For<IHtmlHelper, IViewContextAware>(), urlHelperFactory);
            helper.Context = new ViewContext();

            urlHelperFactory.GetUrlHelper(helper.Context).Returns(urlHelper);
            urlHelper.Content(Arg.Any<string>()).Returns(info => info[0].ToString().TrimStart('~'));

            helper.Process(default, tagHelperOutput);

            helper.Context.ViewData
                .Should().ContainKey("AllLinks")
                .WhichValue.Should().BeEquivalentTo(new HashSet<string>
                {
                    "/lib/bootstrap-markdown/css/bootstrap-markdown.min.css"
                });
        }

        [Fact]
        public void ProcessShouldSetAllScriptsWithDependentScriptUrls()
        {
            var tagHelperOutput = new TagHelperOutput(default, new TagHelperAttributeList(),
                (_, __) => Task.FromResult<TagHelperContent>(new DefaultTagHelperContent()));

            var urlHelperFactory = Substitute.For<IUrlHelperFactory>();
            var urlHelper = Substitute.For<IUrlHelper>();

            var helper = GetBlogEditorTagHelper(Substitute.For<IHtmlHelper, IViewContextAware>(), urlHelperFactory);
            helper.Context = new ViewContext();

            urlHelperFactory.GetUrlHelper(helper.Context).Returns(urlHelper);
            urlHelper.Content(Arg.Any<string>()).Returns(info => info[0].ToString().TrimStart('~'));

            helper.Process(default, tagHelperOutput);

            helper.Context.ViewData
                .Should().ContainKey("AllScripts")
                .WhichValue.Should().BeEquivalentTo(new HashSet<string>
                {
                    "/lib/marked/marked.min.js",
                    "/lib/bootstrap-markdown/js/bootstrap-markdown.js"
                });
        }

        [Fact]
        public void ProcessShouldSetHtmlContentWithView()
        {
            var tagHelperOutput = new TagHelperOutput(default, new TagHelperAttributeList(),
                (_, __) => Task.FromResult<TagHelperContent>(new DefaultTagHelperContent()));

            var urlHelperFactory = Substitute.For<IUrlHelperFactory>();
            var urlHelper = Substitute.For<IUrlHelper>();

            var htmlHelper = Substitute.For<IHtmlHelper, IViewContextAware>();

            var helper = GetBlogEditorTagHelper(htmlHelper, urlHelperFactory);
            helper.Context = new ViewContext();

            htmlHelper.PartialAsync("_BlogEditor", Arg.Any<object>(), helper.Context.ViewData)
                .Returns(Task.FromResult((IHtmlContent) new StringHtmlContent("test")));

            urlHelperFactory.GetUrlHelper(helper.Context).Returns(urlHelper);

            helper.Process(default, tagHelperOutput);

            var writer = new StringWriter();

            tagHelperOutput.Content.WriteTo(writer, HtmlEncoder.Default);

            writer.ToString().Should().BeEquivalentTo("test");
        }

        [Fact]
        public void ProcessShouldSetHtmlFieldPrefix()
        {
            var tagHelperOutput = new TagHelperOutput(default, new TagHelperAttributeList(),
                (_, __) => Task.FromResult<TagHelperContent>(new DefaultTagHelperContent()));

            var urlHelperFactory = Substitute.For<IUrlHelperFactory>();
            var urlHelper = Substitute.For<IUrlHelper>();

            var prefixGenerator = Substitute.For<IPrefixGenerator>();
            prefixGenerator.Generate().Returns("test");

            var helper = GetBlogEditorTagHelper(Substitute.For<IHtmlHelper, IViewContextAware>(), urlHelperFactory,
                prefixGenerator);
            helper.Context = new ViewContext();

            urlHelperFactory.GetUrlHelper(helper.Context).Returns(urlHelper);

            helper.Process(default, tagHelperOutput);

            helper.Context.ViewData.TemplateInfo.HtmlFieldPrefix.Should().BeEquivalentTo("test");
        }

        [Fact]
        public void ProcessShouldSetInlineScriptsWithDependentScriptUrls()
        {
            var tagHelperOutput = new TagHelperOutput(default, new TagHelperAttributeList(),
                (_, __) => Task.FromResult<TagHelperContent>(new DefaultTagHelperContent()));

            var urlHelperFactory = Substitute.For<IUrlHelperFactory>();
            var urlHelper = Substitute.For<IUrlHelper>();

            var helper = GetBlogEditorTagHelper(Substitute.For<IHtmlHelper, IViewContextAware>(), urlHelperFactory);
            helper.Context = new ViewContext();

            urlHelperFactory.GetUrlHelper(helper.Context).Returns(urlHelper);

            helper.Process(default, tagHelperOutput);

            helper.Context.ViewData
                .Should().ContainKey("InlineScripts")
                .WhichValue.Should().BeEquivalentTo($@"<script type=""text/javascript"">
                                                    $('#').markdown({{
                                                                savable: true,
                                                        onChange: function() {{
                                                                    Prism.highlightAll();
                                                                }},
                                                        onSave: function() {{
                                                            $('#').submit();
                                                                }}
                                                            }})
                                                </script>");
        }

        [Fact]
        public void ProcessShouldSetTagNameToAnEmptyString()
        {
            var tagHelperOutput = new TagHelperOutput(default, new TagHelperAttributeList(),
                (_, __) => Task.FromResult<TagHelperContent>(new DefaultTagHelperContent()));

            var urlHelperFactory = Substitute.For<IUrlHelperFactory>();
            var urlHelper = Substitute.For<IUrlHelper>();

            var helper = GetBlogEditorTagHelper(Substitute.For<IHtmlHelper, IViewContextAware>(), urlHelperFactory);
            helper.Context = new ViewContext();

            urlHelperFactory.GetUrlHelper(helper.Context).Returns(urlHelper);

            tagHelperOutput.TagName = "test";

            helper.Process(default, tagHelperOutput);

            tagHelperOutput.TagName.Should().BeEmpty();
        }
    }
}
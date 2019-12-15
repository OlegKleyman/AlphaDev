using System.Collections.Generic;
using System.IO;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using AlphaDev.Web.Core.Support;
using AlphaDev.Web.Core.TagHelpers;
using FluentAssertions;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using NSubstitute;
using Xunit;

namespace AlphaDev.Web.Core.Tests.Unit.TagHelpers
{
    public class EditorTagHelperTests
    {
        [NotNull]
        private EditorTagHelper GetEditorTagHelperHelper(IHtmlHelper? htmlHelper, IUrlHelperFactory? urlHelperFactory,
            EditorView editorView) => Substitute.For<EditorTagHelper>(htmlHelper, urlHelperFactory, editorView);

        [NotNull]
        private EditorTagHelper GetEditorTagHelperHelper(IHtmlHelper? htmlHelper, IUrlHelperFactory? urlHelperFactory) =>
            GetEditorTagHelperHelper(htmlHelper, urlHelperFactory,
                new EditorView("testView", "testPrefix", "testElementName"));

        [Fact]
        public void ContextShouldGetAndSetViewContext()
        {
            var helper = GetEditorTagHelperHelper(default, default);

            var context = new ViewContext();
            helper.Context = context;
            helper.Context.Should().Be(context);
        }

        [Fact]
        public void ModelShouldGetAndSetModel()
        {
            var helper = GetEditorTagHelperHelper(default, default);

            const int model = 1;
            helper.Model = 1;
            helper.Model.Should().Be(model);
        }

        [Fact]
        public void ProcessShouldSetAllLinksWithDependentLinkUrls()
        {
            var tagHelperOutput = new TagHelperOutput(default, new TagHelperAttributeList(),
                (_, __) => Task.FromResult<TagHelperContent>(new DefaultTagHelperContent()));

            var urlHelperFactory = Substitute.For<IUrlHelperFactory>();
            var urlHelper = Substitute.For<IUrlHelper>();

            var helper = GetEditorTagHelperHelper(Substitute.For<IHtmlHelper, IViewContextAware>(), urlHelperFactory);
            helper.Context = new ViewContext();

            urlHelperFactory.GetUrlHelper(helper.Context).Returns(urlHelper);
            urlHelper.Content(Arg.Any<string>()).Returns(info => info[0].ToString()?.TrimStart('~'));

            helper.Process(default, tagHelperOutput);

            helper.Context.ViewData
                  .Should()
                  .ContainKey("AllLinks")
                  .WhichValue.Should()
                  .BeEquivalentTo(new HashSet<string>
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

            var helper = GetEditorTagHelperHelper(Substitute.For<IHtmlHelper, IViewContextAware>(), urlHelperFactory);
            helper.Context = new ViewContext();

            urlHelperFactory.GetUrlHelper(helper.Context).Returns(urlHelper);
            urlHelper.Content(Arg.Any<string>()).Returns(info => info[0].ToString()?.TrimStart('~'));

            helper.Process(default, tagHelperOutput);

            helper.Context.ViewData
                  .Should()
                  .ContainKey("AllScripts")
                  .WhichValue.Should()
                  .BeEquivalentTo(new HashSet<string>
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

            var editorView = new EditorView("testView", "testPrefix", "testElementName");
            var helper = GetEditorTagHelperHelper(htmlHelper, urlHelperFactory, editorView);
            helper.Context = new ViewContext();

            // ReSharper disable once Mvc.PartialViewNotResolved - no need for a valid view in unit test
            htmlHelper.PartialAsync(editorView.Name, Arg.Any<object>(), helper.Context.ViewData)
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

            var editorView = new EditorView("testView", "testPrefix", "testElementName");
            var helper = GetEditorTagHelperHelper(Substitute.For<IHtmlHelper, IViewContextAware>(), urlHelperFactory,
                editorView);
            helper.Context = new ViewContext();

            urlHelperFactory.GetUrlHelper(helper.Context).Returns(urlHelper);

            helper.Process(default, tagHelperOutput);

            helper.Context.ViewData.TemplateInfo.HtmlFieldPrefix.Should().BeEquivalentTo(editorView.Prefix);
        }

        [Fact]
        public void ProcessShouldSetInlineScriptsWithDependentScriptUrls()
        {
            var tagHelperOutput = new TagHelperOutput(default, new TagHelperAttributeList(),
                (_, __) => Task.FromResult<TagHelperContent>(new DefaultTagHelperContent()));

            var urlHelperFactory = Substitute.For<IUrlHelperFactory>();
            var urlHelper = Substitute.For<IUrlHelper>();

            var helper = GetEditorTagHelperHelper(Substitute.For<IHtmlHelper, IViewContextAware>(), urlHelperFactory);
            helper.Context = new ViewContext();

            urlHelperFactory.GetUrlHelper(helper.Context).Returns(urlHelper);

            helper.Process(default, tagHelperOutput);

            helper.Context.ViewData
                  .Should()
                  .ContainKey("InlineScripts")
                  .WhichValue.Should()
                  .BeEquivalentTo(@"<script type=""text/javascript"">
                                                    $('#').markdown({
                                                                savable: true,
                                                        onChange: function() {
                                                                    Prism.highlightAll();
                                                                },
                                                        onSave: function() {
                                                            $('#').submit();
                                                                }
                                                            })
                                                </script>");
        }

        [Fact]
        public void ProcessShouldSetTagNameToAnEmptyString()
        {
            var tagHelperOutput = new TagHelperOutput(default, new TagHelperAttributeList(),
                (_, __) => Task.FromResult<TagHelperContent>(new DefaultTagHelperContent()));

            var urlHelperFactory = Substitute.For<IUrlHelperFactory>();
            var urlHelper = Substitute.For<IUrlHelper>();

            var helper = GetEditorTagHelperHelper(Substitute.For<IHtmlHelper, IViewContextAware>(), urlHelperFactory);
            helper.Context = new ViewContext();

            urlHelperFactory.GetUrlHelper(helper.Context).Returns(urlHelper);

            tagHelperOutput.TagName = "test";

            helper.Process(default, tagHelperOutput);

            tagHelperOutput.TagName.Should().BeEmpty();
        }
    }
}
using System.Threading.Tasks;
using AlphaDev.Core;
using AlphaDev.Web.TagHelpers;
using FluentAssertions;
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
        [Fact]
        public void ConstructorShouldInitializeBlogEditorTagHelperWithTheCorrectEditorElementName()
        {
            var tagHelperOutput = new TagHelperOutput(default, new TagHelperAttributeList(),
                (_, __) => Task.FromResult<TagHelperContent>(new DefaultTagHelperContent()));

            var htmlHelper = Substitute.For<IHtmlHelper, IViewContextAware>();
            htmlHelper.Id("Content").Returns("Content");
            var helper = new BlogEditorTagHelper(htmlHelper, Substitute.For<IUrlHelperFactory>(),
                Substitute.For<IPrefixGenerator>());
            helper.Context = new ViewContext();

            helper.Process(default, tagHelperOutput);
            // ReSharper disable once Mvc.PartialViewNotResolved - no need for a valid view in unit test
            // ReSharper disable once MustUseReturnValue - don't care about return value
            helper.Context.ViewData
                .Should().ContainKey("InlineScripts")
                .WhichValue.Should().BeEquivalentTo(@"<script type=""text/javascript"">
                                                    $('#Content').markdown({
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
        public void ConstructorShouldInitializeBlogEditorTagHelperWithTheCorrectViewName()
        {
            var tagHelperOutput = new TagHelperOutput(default, new TagHelperAttributeList(),
                (_, __) => Task.FromResult<TagHelperContent>(new DefaultTagHelperContent()));

            var htmlHelper = Substitute.For<IHtmlHelper, IViewContextAware>();

            var helper = new BlogEditorTagHelper(htmlHelper, Substitute.For<IUrlHelperFactory>(),
                Substitute.For<IPrefixGenerator>());
            helper.Context = new ViewContext();

            helper.Process(default, tagHelperOutput);
            // ReSharper disable once Mvc.PartialViewNotResolved - no need for a valid view in unit test
            // ReSharper disable once MustUseReturnValue - don't care about return value
            htmlHelper.Received(1).PartialAsync("_BlogEditor", Arg.Any<object>(), helper.Context.ViewData);
        }

        [Fact]
        public void ConstructorShouldInitializeBlogEditorTagHelperWithTheHtmlHelperArgument()
        {
            var tagHelperOutput = new TagHelperOutput(default, new TagHelperAttributeList(),
                (_, __) => Task.FromResult<TagHelperContent>(new DefaultTagHelperContent()));

            var htmlHelper = Substitute.For<IHtmlHelper, IViewContextAware>();

            var helper = new BlogEditorTagHelper(htmlHelper, Substitute.For<IUrlHelperFactory>(),
                Substitute.For<IPrefixGenerator>());
            helper.Context = new ViewContext();

            helper.Process(default, tagHelperOutput);
            // ReSharper disable once Mvc.PartialViewNotResolved - no need for a valid view in unit test
            // ReSharper disable once MustUseReturnValue - don't care about return value
            htmlHelper.Received(1).PartialAsync(Arg.Any<string>(), Arg.Any<object>(), helper.Context.ViewData);
        }

        [Fact]
        public void ConstructorShouldInitializeBlogEditorTagHelperWithThePrefixGeneratorResult()
        {
            var tagHelperOutput = new TagHelperOutput(default, new TagHelperAttributeList(),
                (_, __) => Task.FromResult<TagHelperContent>(new DefaultTagHelperContent()));

            var prefixGenerator = Substitute.For<IPrefixGenerator>();
            const string prefix = "test";
            prefixGenerator.Generate().Returns(prefix);
            var helper = new BlogEditorTagHelper(Substitute.For<IHtmlHelper, IViewContextAware>(),
                Substitute.For<IUrlHelperFactory>(), prefixGenerator);
            helper.Context = new ViewContext();
            helper.Process(default, tagHelperOutput);

            helper.Context.ViewData.TemplateInfo.HtmlFieldPrefix.Should().BeEquivalentTo(prefix);
        }

        [Fact]
        public void ConstructorShouldInitializeBlogEditorTagHelperWithTheUrlHelperFactoryArgument()
        {
            var tagHelperOutput = new TagHelperOutput(default, new TagHelperAttributeList(),
                (_, __) => Task.FromResult<TagHelperContent>(new DefaultTagHelperContent()));

            var urlHelperFactory = Substitute.For<IUrlHelperFactory>();
            var urlHelper = Substitute.For<IUrlHelper>();

            var helper = new BlogEditorTagHelper(Substitute.For<IHtmlHelper, IViewContextAware>(), urlHelperFactory,
                Substitute.For<IPrefixGenerator>());
            helper.Context = new ViewContext();
            urlHelperFactory.GetUrlHelper(helper.Context).Returns(urlHelper);
            helper.Process(default, tagHelperOutput);
            urlHelper.Received().Content(Arg.Any<string>());
        }
    }
}
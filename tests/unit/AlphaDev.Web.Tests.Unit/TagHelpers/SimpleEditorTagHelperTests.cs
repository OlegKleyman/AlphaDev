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
    public class SimpleEditorTagHelperTests
    {
        [Fact]
        public void ConstructorShouldInitializeSimpleEditorTagHelperWithTheCorrectEditorElementName()
        {
            var tagHelperOutput = new TagHelperOutput(default, new TagHelperAttributeList(),
                (_, __) => Task.FromResult<TagHelperContent>(new DefaultTagHelperContent()));

            var htmlHelper = Substitute.For<IHtmlHelper, IViewContextAware>();
            htmlHelper.Id(Arg.Any<string>()).Returns(info => info[0].ToString());
            var helper = new SimpleEditorTagHelper(htmlHelper, Substitute.For<IUrlHelperFactory>(),
                Substitute.For<IPrefixGenerator>()) { Context = new ViewContext() };

            helper.Process(default, tagHelperOutput);
            helper.Context.ViewData
                .Should().ContainKey("InlineScripts")
                .WhichValue.Should().BeEquivalentTo(@"<script type=""text/javascript"">
                                                    $('#Value').markdown({
                                                                savable: true,
                                                        onChange: function() {
                                                                    Prism.highlightAll();
                                                                },
                                                        onSave: function() {
                                                            $('#editForm').submit();
                                                                }
                                                            })
                                                </script>");
        }

        [Fact]
        public void ConstructorShouldInitializeSimpleEditorTagHelperWithTheCorrectViewName()
        {
            var tagHelperOutput = new TagHelperOutput(default, new TagHelperAttributeList(),
                (_, __) => Task.FromResult<TagHelperContent>(new DefaultTagHelperContent()));

            var htmlHelper = Substitute.For<IHtmlHelper, IViewContextAware>();

            var helper = new SimpleEditorTagHelper(htmlHelper, Substitute.For<IUrlHelperFactory>(),
                Substitute.For<IPrefixGenerator>()) { Context = new ViewContext() };

            helper.Process(default, tagHelperOutput);
            // ReSharper disable once Mvc.PartialViewNotResolved - no need for a valid view in unit test
            // ReSharper disable once MustUseReturnValue - don't care about return value
            htmlHelper.Received(1).PartialAsync("_simpleEditor", Arg.Any<object>(), helper.Context.ViewData);
        }

        [Fact]
        public void ConstructorShouldInitializeSimpleEditorTagHelperWithTheHtmlHelperArgument()
        {
            var tagHelperOutput = new TagHelperOutput(default, new TagHelperAttributeList(),
                (_, __) => Task.FromResult<TagHelperContent>(new DefaultTagHelperContent()));

            var htmlHelper = Substitute.For<IHtmlHelper, IViewContextAware>();

            var helper = new SimpleEditorTagHelper(htmlHelper, Substitute.For<IUrlHelperFactory>(),
                Substitute.For<IPrefixGenerator>()) { Context = new ViewContext() };

            helper.Process(default, tagHelperOutput);
            // ReSharper disable once Mvc.PartialViewNotResolved - no need for a valid view in unit test
            // ReSharper disable once MustUseReturnValue - don't care about return value
            htmlHelper.Received(1).PartialAsync(Arg.Any<string>(), Arg.Any<object>(), helper.Context.ViewData);
        }

        [Fact]
        public void ConstructorShouldInitializeSimpleEditorTagHelperWithThePrefixGeneratorResult()
        {
            var tagHelperOutput = new TagHelperOutput(default, new TagHelperAttributeList(),
                (_, __) => Task.FromResult<TagHelperContent>(new DefaultTagHelperContent()));

            var prefixGenerator = Substitute.For<IPrefixGenerator>();
            const string prefix = "test";
            prefixGenerator.Generate().Returns(prefix);
            var helper = new SimpleEditorTagHelper(Substitute.For<IHtmlHelper, IViewContextAware>(),
                Substitute.For<IUrlHelperFactory>(), prefixGenerator) { Context = new ViewContext() };
            helper.Process(default, tagHelperOutput);

            helper.Context.ViewData.TemplateInfo.HtmlFieldPrefix.Should().BeEquivalentTo(prefix);
        }

        [Fact]
        public void ConstructorShouldInitializeSimpleEditorTagHelperWithTheUrlHelperFactoryArgument()
        {
            var tagHelperOutput = new TagHelperOutput(default, new TagHelperAttributeList(),
                (_, __) => Task.FromResult<TagHelperContent>(new DefaultTagHelperContent()));

            var urlHelperFactory = Substitute.For<IUrlHelperFactory>();
            var urlHelper = Substitute.For<IUrlHelper>();

            var helper = new SimpleEditorTagHelper(Substitute.For<IHtmlHelper, IViewContextAware>(), urlHelperFactory,
                Substitute.For<IPrefixGenerator>()) { Context = new ViewContext() };
            urlHelperFactory.GetUrlHelper(helper.Context).Returns(urlHelper);
            helper.Process(default, tagHelperOutput);
            urlHelper.Received().Content(Arg.Any<string>());
        }
    }
}
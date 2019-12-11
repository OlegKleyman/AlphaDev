using System;
using System.IO;
using System.Linq.Expressions;
using System.Text.Encodings.Web;
using AlphaDev.Web.Extensions;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using NSubstitute;
using Optional;
using Xunit;

namespace AlphaDev.Web.Tests.Unit.Extensions
{
    public class HtmlHelperOptionExtensionsTests
    {
        [Fact]
        public void ActionLinkShouldReturnEmptyHtmlContentWhenOptionIsNone()
        {
            var target = Option.None<string>();
            var helper = Substitute.For<IHtmlHelper<object>>();

            var content = helper.ActionLink(target, default, default, default, default, default);

            var writer = new StringWriter();
            content.WriteTo(writer, HtmlEncoder.Default);
            writer.ToString().Should().BeEquivalentTo(string.Empty);
        }

        [Fact]
        public void ActionLinkShouldReturnFormattedHtmlContentWhenOptionIsNotEmpty()
        {
            var target = Option.Some("test");
            var helper = Substitute.For<IHtmlHelper<object>>();
            // ReSharper disable once Mvc.ActionNotResolved - real action not needed
            // ReSharper disable once Mvc.ControllerNotResolved - real controller not needed
            helper.ActionLink("text", "action", "controller", null, null, null, "routeValue", Arg.Any<object>())
                  .Returns(new StringHtmlContent("test"));

            var content = helper.ActionLink(target, "text", "action", "controller", x => "routeValue", default);

            var writer = new StringWriter();
            content.WriteTo(writer, HtmlEncoder.Default);
            writer.ToString().Should().BeEquivalentTo("test");
        }

        [Fact]
        public void DisplayForShouldReturnEmptyHtmlContentWhenOptionIsNone()
        {
            var target = Option.None<string>();
            var helper = Substitute.For<IHtmlHelper<object>>();

            var content = helper.DisplayFor(target);

            var writer = new StringWriter();
            content.WriteTo(writer, HtmlEncoder.Default);
            writer.ToString().Should().BeEquivalentTo(string.Empty);
        }

        [Fact]
        public void DisplayForShouldReturnFormattedHtmlContentWhenOptionIsNotEmpty()
        {
            var target = Option.Some("test");
            var helper = Substitute.For<IHtmlHelper<object>>();
            helper.DisplayFor(Arg.Any<Expression<Func<object, string>>>(), Arg.Any<string>(), Arg.Any<string>(),
                      Arg.Any<object>())
                  .Returns(new StringHtmlContent("test"));

            var content = helper.DisplayFor(target);

            var writer = new StringWriter();
            content.WriteTo(writer, HtmlEncoder.Default);
            writer.ToString().Should().BeEquivalentTo("test");
        }

        [Fact]
        public void HiddenShouldReturnEmptyHtmlContentWhenOptionIsNone()
        {
            var target = Option.None<string>();
            var helper = Substitute.For<IHtmlHelper<object>>();

            var content = HtmlHelperOptionExtensions.Hidden(helper, default, target, default);

            var writer = new StringWriter();
            content.WriteTo(writer, HtmlEncoder.Default);
            writer.ToString().Should().BeEquivalentTo(string.Empty);
        }

        [Fact]
        public void HiddenShouldReturnFormattedHtmlContentWhenOptionIsNotEmpty()
        {
            var target = Option.Some("test");
            var helper = Substitute.For<IHtmlHelper<object>>();
            helper.Hidden("test", "test", Arg.Any<object>()).Returns(new StringHtmlContent("test"));

            var content = HtmlHelperOptionExtensions.Hidden(helper, "test", target, default);

            var writer = new StringWriter();
            content.WriteTo(writer, HtmlEncoder.Default);
            writer.ToString().Should().BeEquivalentTo("test");
        }
    }
}
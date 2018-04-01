using System;
using System.IO;
using System.Linq.Expressions;
using System.Text;
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
        public void DisplayForShouldReturnEmptyHtmlContentWhenOptionIsNone()
        {
            var target = Option.None<string>();
            var helper = Substitute.For<IHtmlHelper<object>>();
            helper.DisplayFor(Arg.Any<Expression<Func<object, string>>>(), Arg.Any<string>(), Arg.Any<string>(),
                Arg.Any<object>()).Returns(new StringHtmlContent("test"));

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
                Arg.Any<object>()).Returns(new StringHtmlContent("test"));

            var content = helper.DisplayFor(target);

            var writer = new StringWriter();
            content.WriteTo(writer, HtmlEncoder.Default);
            writer.ToString().Should().BeEquivalentTo("test");
        }
    }
}

﻿namespace AlphaDev.Web.Tests.Unit.TagHelpers
{
    using AlphaDev.Web.TagHelpers;

    using FluentAssertions;

    using Microsoft.AspNetCore.Razor.TagHelpers;

    using Xunit;

    public class DisplayTagHelperTests
    {
        [Theory]
        [InlineData(true, "display: inline;")]
        [InlineData(false, "display: none;")]
        public void ProcessShouldCreateElementWithStyleDisplayPropertyCorrespondingToTheArgument(bool value, string expectedDisplayValue)
        {
            var sut = GetDisplayTagHelper();

            sut.Value = value;
            var tagHelperOutput = new TagHelperOutput(default(string), new TagHelperAttributeList(), (_, __) =>  null);

            sut.Process(null, tagHelperOutput);

            tagHelperOutput.Attributes["style"].Value.Should().BeOfType<string>().Which.ShouldBeEquivalentTo(expectedDisplayValue);
        }

        [Fact]
        public void ProcessShouldCreateDivElement()
        {
            var sut = GetDisplayTagHelper();
            
            var tagHelperOutput = new TagHelperOutput(default(string), new TagHelperAttributeList(), (_, __) => null);

            sut.Process(null, tagHelperOutput);

            tagHelperOutput.TagName.ShouldBeEquivalentTo("div");
        }

        [Fact]
        public void ProcessShouldSetValuePropertyToTrueByDefault() => GetDisplayTagHelper().Value
            .ShouldBeEquivalentTo(true);

        private DisplayTagHelper GetDisplayTagHelper() => new DisplayTagHelper();
    }
}

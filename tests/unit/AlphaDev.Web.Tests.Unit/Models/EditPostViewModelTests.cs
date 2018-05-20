using System;
using AlphaDev.Web.Models;
using FluentAssertions;
using Xunit;

namespace AlphaDev.Web.Tests.Unit.Models
{
    public class EditPostViewModelTests
    {
        [Fact]
        public void GetConstructorShouldSetProperties()
        {
            const string title = "title";
            const string content = "content";
            var dates = new DatesViewModel(new DateTime(2018, 1, 1), default);

            var model = new EditPostViewModel(title, content, dates);

            model.Should().BeEquivalentTo(new {Title = title, Content = content, Dates = dates});
        }

        [Fact]
        public void PostConstructorShouldSetProperties()
        {
            const string title = "title";
            const string content = "content";

            var model = new EditPostViewModel(title, content, default);

            model.Should().BeEquivalentTo(new {Title = title, Content = content});
        }
    }
}
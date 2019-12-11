using System;
using AlphaDev.Web.Models;
using FluentAssertions;
using Optional;
using Xunit;

namespace AlphaDev.Web.Tests.Unit.Models
{
    public class BlogEditorViewModelTests
    {
        [Fact]
        public void ConstructorShouldInitializeAllProperties()
        {
            const string title = "title";
            const string content = "content";
            var dates = new DatesViewModel(new DateTime(2011, 1, 20), new DateTime(2013, 7, 14).Some()).Some();

            var model = new BlogEditorViewModel(dates, content, title);

            model.Should()
                 .BeEquivalentTo(new
                 {
                     Title = title,
                     Content = content,
                     Dates = dates
                 });
        }
    }
}
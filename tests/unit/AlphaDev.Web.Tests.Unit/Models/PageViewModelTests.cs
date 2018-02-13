using System;
using System.Collections.Generic;
using System.Text;
using AlphaDev.Web.Models;
using FluentAssertions;
using NSubstitute;
using Optional;
using Xunit;

namespace AlphaDev.Web.Tests.Unit.Models
{
    public class PageViewModelTests
    {
        [Fact]
        public void ConstructorShouldSetProperties()
        {
            var title = "test title";

            var dates = Substitute.For<PageViewModel>(title);
            
            dates.Title.ShouldBeEquivalentTo(title);
        }
    }
}

using System;
using AlphaDev.Web.Models;
using FluentAssertions;
using Optional;
using Xunit;

namespace AlphaDev.Web.Tests.Unit.Models
{
    public class DatesViewModelTests
    {
        [Fact]
        public void ConstructorShouldSetProperties()
        {
            var created = new DateTime(2017, 1, 1);
            var modified = Option.Some(new DateTime(2017, 2, 1));

            var dates = new DatesViewModel(created, modified);

            dates.Created.ShouldBeEquivalentTo(created);
            dates.Modified.ShouldBeEquivalentTo(modified);
        }
    }
}
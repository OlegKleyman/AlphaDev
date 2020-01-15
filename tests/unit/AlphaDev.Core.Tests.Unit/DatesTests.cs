using System;
using AlphaDev.Services;
using FluentAssertions;
using Optional;
using Xunit;

namespace AlphaDev.Core.Tests.Unit
{
    public class DatesTests
    {
        [Fact]
        public void ConstructorShouldSetProperties()
        {
            var created = new DateTime(2017, 1, 1);
            var modified = Option.Some(new DateTime(2017, 2, 1));

            var dates = new Dates(created, modified);

            dates.Created.Should().Be(created);
            dates.Modified.Should().BeEquivalentTo(modified);
        }
    }
}
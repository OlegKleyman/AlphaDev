using System;
using System.Linq;
using FluentAssertions;
using Xunit;

namespace AlphaDev.BlogServices.Core.Tests.Unit
{
    public class ObjectNotFoundExceptionTests
    {
        [Fact]
        public void CriteriaReturnsTheCriteriaUsedWhenTryingToRetrieveTheObject()
        {
            new ObjectNotFoundException(typeof(string), ("id", 2), ("name", "alpha"))
                .Criteria.SelectMany(pair => pair.Value.Select(x => (pair.Key, x)))
                .Should()
                .BeEquivalentTo(("id", 2), ("name", "alpha"));
        }

        [Fact]
        public void MessageReturnsMessageWithDescribingTheObjectMissingAndCriteriaInvolvedInTryingToFindIt()
        {
            var exception = new ObjectNotFoundException(typeof(string), ("id", 2), ("name", "alpha"));
            exception.Message.Should()
                     .Be(
                         $"System.String was not found based on the criteria.{Environment.NewLine}id:2{Environment.NewLine}name:alpha");
        }

        [Fact]
        public void TypeReturnsTheTypeTheExceptionWasInitializedWith()
        {
            new ObjectNotFoundException(typeof(string)).Type.Should().Be<string>();
        }
    }
}
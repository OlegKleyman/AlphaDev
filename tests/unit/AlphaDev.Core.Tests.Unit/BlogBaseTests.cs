using System;
using FluentAssertions;
using Optional;
using Xunit;

namespace AlphaDev.Core.Tests.Unit
{
    public class BlogBaseTests
    {
        [Fact]
        public void EmptyShouldReturnEmptyBlog()
        {
            BlogBase.Empty.ShouldBeEquivalentTo(
                new
                {
                    Id = default(int),
                    Title = string.Empty,
                    Content = string.Empty,
                    Dates = new Dates(default, Option.None<DateTime>())
                });
        }
    }
}
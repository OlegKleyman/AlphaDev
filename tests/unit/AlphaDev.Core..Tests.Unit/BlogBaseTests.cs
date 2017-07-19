namespace AlphaDev.Core.Tests.Unit
{
    using System;

    using AppDev.Core;

    using FluentAssertions;

    using Optional;

    using Xunit;

    public class BlogBaseTests
    {
        [Fact]
        public void EmptyShouldReturnEmptyBlog() => BlogBase.Empty.ShouldBeEquivalentTo(
            new
                {
                    Title = string.Empty,
                    Content = string.Empty,
                    Dates = new Dates(default(DateTime), Option.None<DateTime>())
                });
    }
}

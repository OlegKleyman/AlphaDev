using System;
using FluentAssertions;
using Xunit;

namespace AlphaDev.BlogServices.Core.Tests.Unit
{
    public class ObjectNotFoundExceptionOfTTests
    {
        [Fact]
        public void ConstructorInitializesObjectNotFoundExceptionWithTheGenericType()
        {
            new ObjectNotFoundException<string>().Type.Should().Be<string>();
        }
    }
}
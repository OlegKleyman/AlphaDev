using System;
using FluentAssertions;
using Xunit;

namespace AlphaDev.Services.Tests.Unit
{
    public class ObjectNotFoundExceptionOfTTests
    {
        [Fact]
        public void ConstructorInitializesObjectNotFoundExceptionWithTheGenericType()
        {
            ((Type) new ObjectNotFoundException<string>().Type).Should().Be<string>();
        }
    }
}
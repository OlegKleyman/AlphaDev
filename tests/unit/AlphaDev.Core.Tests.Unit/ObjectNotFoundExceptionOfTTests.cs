using FluentAssertions;
using Xunit;

namespace AlphaDev.Core.Tests.Unit
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
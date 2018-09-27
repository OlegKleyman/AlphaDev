using AlphaDev.Web.Support;
using FluentAssertions;
using Xunit;

namespace AlphaDev.Web.Tests.Unit.Support
{
    public class EditorViewTests
    {
        [Fact]
        public void ConstructorShouldSetRequiredProperties()
        {
            const string name = "name";
            const string prefix = "prefix";
            const string editorElementName = "editorElementName";

            new EditorView(name, prefix, editorElementName).Should().BeEquivalentTo(new
            {
                Name = name,
                Prefix = prefix,
                EditorElementName = editorElementName
            });
        }
    }
}
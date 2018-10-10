using AlphaDev.Web.Models;
using FluentAssertions;
using Xunit;

namespace AlphaDev.Web.Tests.Unit.Models
{
    public class DisplayValueViewModelTests
    {
        [Fact]
        public void ConstructorShouldInitializeDisplayValueViewModel()
        {
            const string editAction = "editAction";
            const string controller = "controller";
            const string value = "value";
            const string title = "title";
            var model = new DisplayValueViewModel(editAction, controller, value, title);
            model.Should().BeEquivalentTo(new
                { EditAction = editAction, Controller = controller, Value = value, Title = title });
        }
    }
}
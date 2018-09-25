using AlphaDev.Web.Support;
using FluentAssertions;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Primitives;
using NSubstitute;
using Xunit;

namespace AlphaDev.Web.Tests.Unit.Support
{
    public class AboutCreateModelBinderTests
    {
        [NotNull]
        private static ModelBindingContext GetContext([CanBeNull] IValueProvider valueProvider)
        {
            var context = new DefaultModelBindingContext
            {
                ModelState = new ModelStateDictionary(),
                ValueProvider = valueProvider ?? Substitute.For<IValueProvider>()
            };

            return context;
        }

        [NotNull]
        private AboutCreateModelBinderMock GetAboutCreateModelBinder()
        {
            return new AboutCreateModelBinderMock();
        }

        public class AboutCreateModelBinderMock : AboutCreateModelBinder
        {
            public void BindModelMock([NotNull] ModelBindingContext context)
            {
                BindModel(context);
            }
        }

        [Fact]
        public void BindModelShouldSetModelWithWithValue()
        {
            var binder = GetAboutCreateModelBinder();

            var valueProvider = Substitute.For<IValueProvider>();

            valueProvider.GetValue("Value").Returns(new ValueProviderResult(new StringValues("value")));

            var context = GetContext(valueProvider);

            binder.BindModelMock(context);
            context.Result.Model.Should().BeEquivalentTo(new { Value = "value" });
        }

        [Fact]
        public void BindModelShouldSetModelWithValueEmptyWhenNoValueExists()
        {
            var binder = GetAboutCreateModelBinder();

            var valueProvider = Substitute.For<IValueProvider>();

            var context = GetContext(valueProvider);

            binder.BindModelMock(context);
            context.Result.Model.Should().BeEquivalentTo(new { Value = string.Empty });
        }
    }
}
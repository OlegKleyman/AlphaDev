using AlphaDev.Web.Support;
using FluentAssertions;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Primitives;
using NSubstitute;
using Xunit;

namespace AlphaDev.Web.Tests.Unit.Support
{
    public class CreatePostModelBinderTests
    {
        [NotNull]
        private static PrefixModelBindingContext GetContext([CanBeNull] IValueProvider valueProvider)
        {
            var context = new DefaultModelBindingContext
            {
                ModelState = new ModelStateDictionary(),
                ValueProvider = valueProvider ?? Substitute.For<IValueProvider>()
            };

            return new PrefixModelBindingContext(context, default);
        }

        [NotNull]
        private MockCreatePostModelBinder GetCreatePostModelBinder()
        {
            return new MockCreatePostModelBinder();
        }

        public class MockCreatePostModelBinder : CreatePostModelBinder
        {
            public void BindModelMock([NotNull] PrefixModelBindingContext context)
            {
                BindModel(context);
            }
        }

        [Fact]
        public void BindModelAsyncShouldSetModelWithTitleAndContent()
        {
            var binder = GetCreatePostModelBinder();

            var valueProvider = Substitute.For<IValueProvider>();

            valueProvider.GetValue("Title").Returns(new ValueProviderResult(new StringValues("title")));
            valueProvider.GetValue("Content").Returns(new ValueProviderResult(new StringValues("content")));

            var context = GetContext(valueProvider);

            binder.BindModelMock(context);

            context.Result.Model.Should().BeEquivalentTo(new {Title = "title", Content = "content"});
        }

        [Fact]
        public void BindModelAsyncShouldSetModelWithTitleAndContentEmptyWhenNoValuesExists()
        {
            var binder = GetCreatePostModelBinder();

            var context = GetContext(Substitute.For<IValueProvider>());

            binder.BindModelMock(context);

            context.Result.Model.Should().BeEquivalentTo(new {Title = string.Empty, Content = string.Empty});
        }

        [Fact]
        public void BindModelAsyncShouldSetModelWithTitleAndContentEmptyWhenValueProviderIsNull()
        {
            var binder = GetCreatePostModelBinder();

            var context = GetContext(default);

            binder.BindModelMock(context);
            context.Result.Model.Should().BeEquivalentTo(new {Title = string.Empty, Content = string.Empty});
        }
    }
}
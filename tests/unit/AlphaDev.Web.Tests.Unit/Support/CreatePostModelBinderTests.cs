using System;
using AlphaDev.Web.Support;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Primitives;
using NSubstitute;
using Xunit;

namespace AlphaDev.Web.Tests.Unit.Support
{
    public class CreatePostModelBinderTests
    {
        private static PrefixModelBindingContext GetContext(IValueProvider valueProvider)
        {
            var context = new DefaultModelBindingContext
            {
                ModelState = new ModelStateDictionary(),
                ValueProvider = valueProvider ?? Substitute.For<IValueProvider>()
            };

            return new PrefixModelBindingContext(context, default);
        }

        private MockCreatePostModelBinder GetCreatePostModelBinder()
        {
            return new MockCreatePostModelBinder();
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

            context.Result.Model.Should().BeEquivalentTo(new { Title = "title", Content = "content" });
        }

        [Fact]
        public void BindModelAsyncShouldSetModelWithTitleAndContentEmptyWhenNoValuesExists()
        {
            var binder = GetCreatePostModelBinder();

            var context = GetContext(Substitute.For<IValueProvider>());

            binder.BindModelMock(context);

            context.Result.Model.Should().BeEquivalentTo(new { Title = string.Empty, Content = string.Empty });
        }

        [Fact]
        public void BindModelAsyncShouldSetModelWithTitleAndContentEmptyWhenValueProviderIsNull()
        {
            var binder = GetCreatePostModelBinder();

            var context = GetContext(default);

            binder.BindModelMock(context);
            context.Result.Model.Should().BeEquivalentTo(new { Title = string.Empty, Content = string.Empty });
        }

        public class MockCreatePostModelBinder : CreatePostModelBinder
        {
            public void BindModelMock(PrefixModelBindingContext context)
            {
                BindModel(context);
            }
        }
    }
}
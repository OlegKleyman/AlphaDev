using System;
using AlphaDev.Web.Support;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Internal;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Primitives;
using NSubstitute;
using Xunit;

namespace AlphaDev.Web.Tests.Unit.Support
{
    public class CreatePostModelBinderTests
    {
        [Fact]
        public void BindModelAsyncShouldSetModelWithTitleAndContent()
        {
            var binder = GetCreatePostModelBinder();

            var context = GetContext();
            context.ValueProvider = Substitute.For<IValueProvider>();
            context.ValueProvider.GetValue("Title").Returns(new ValueProviderResult(new StringValues("title")));
            context.ValueProvider.GetValue("Content").Returns(new ValueProviderResult(new StringValues("content")));

            binder.BindModelAsync(context).GetAwaiter().OnCompleted(() =>
            {
                context.Result.Model.Should().BeEquivalentTo(new {Title = "title", Content = "content"});
            });
        }

        [Fact]
        public void BindModelAsyncShouldSetModelWithTitleAndContentEmptyWhenNoValuesExists()
        {
            var binder = GetCreatePostModelBinder();

            var context = GetContext();
            context.ValueProvider = Substitute.For<IValueProvider>();

            binder.BindModelAsync(context).GetAwaiter().OnCompleted(() =>
            {
                context.Result.Model.Should().BeEquivalentTo(new { Title = string.Empty, Content = string.Empty });
            });
        }

        [Fact]
        public void BindModelAsyncShouldSetModelWithTitleAndContentEmptyWhenValueProviderIsNull()
        {
            var binder = GetCreatePostModelBinder();

            var context = GetContext();

            binder.BindModelAsync(context).GetAwaiter().OnCompleted(() =>
            {
                context.Result.Model.Should().BeEquivalentTo(new { Title = string.Empty, Content = string.Empty });
            });
        }

        [Fact]
        public void BindModelAsyncShouldThrowArgumentNullExceptionWhenContextIsNull()
        {
            var binder = GetCreatePostModelBinder();
            Action bindModelAsync = () => binder.BindModelAsync(null);
            bindModelAsync.Should().Throw<ArgumentNullException>()
                .WithMessage("Value cannot be null.\r\nParameter name: bindingContext").Which.ParamName.Should()
                .BeEquivalentTo("bindingContext");
        }

        private static DefaultModelBindingContext GetContext()
        {
            var context = new DefaultModelBindingContext();

            return context;
        }

        private CreatePostModelBinder GetCreatePostModelBinder()
        {
            return new CreatePostModelBinder();
        }
    }
}

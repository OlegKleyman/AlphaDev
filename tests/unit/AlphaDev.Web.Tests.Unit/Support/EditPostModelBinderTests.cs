using System;
using AlphaDev.Web.Models;
using AlphaDev.Web.Support;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Primitives;
using NSubstitute;
using Optional;
using Xunit;

namespace AlphaDev.Web.Tests.Unit.Support
{
    public class EditPostModelBinderTests
    {
        private static DefaultModelBindingContext GetContext()
        {
            return new DefaultModelBindingContext
            {
                ModelState = new ModelStateDictionary()
            };
        }

        private EditPostModelBinder GetEditPostModelBinder()
        {
            return new EditPostModelBinder();
        }

        [Fact]
        public async void BindModelAsyncShouldSetModelWithTitleAndContent()
        {
            var binder = GetEditPostModelBinder();

            var context = GetContext();
            context.ValueProvider = Substitute.For<IValueProvider>();
            context.ValueProvider.GetValue("Title").Returns(new ValueProviderResult(new StringValues("title")));
            context.ValueProvider.GetValue("Content").Returns(new ValueProviderResult(new StringValues("content")));
            context.ValueProvider.GetValue("Created").Returns(new ValueProviderResult(new StringValues("1/1/2018")));
            await binder.BindModelAsync(context);
            context.Result.Model.Should().BeEquivalentTo(new { Title = "title", Content = "content" }, options => options.ExcludingMissingMembers());
        }

        [Fact]
        public async void BindModelAsyncShouldSetModelWithoutModificationDateWhenItIsMissing()
        {
            var binder = GetEditPostModelBinder();

            var context = GetContext();
            context.ValueProvider = Substitute.For<IValueProvider>();
            context.ValueProvider.GetValue("Created").Returns(new ValueProviderResult(new StringValues("1/1/2018")));

            await binder.BindModelAsync(context);

            context.Result.Model.Should().BeEquivalentTo(new
            {
                Dates = new DatesViewModel(new DateTime(2018, 1, 1), default)
            });
        }

        [Fact]
        public async void BindModelAsyncShouldSetModelWithModificationDateWhenItExists()
        {
            var binder = GetEditPostModelBinder();

            var context = GetContext();
            context.ValueProvider = Substitute.For<IValueProvider>();
            context.ValueProvider.GetValue("Created").Returns(new ValueProviderResult(new StringValues("1/1/2018")));
            context.ValueProvider.GetValue("Modified").Returns(new ValueProviderResult(new StringValues("2/1/2018")));
            await binder.BindModelAsync(context);
            context.Result.Model.Should().BeEquivalentTo(new
            {
                Dates = new DatesViewModel(new DateTime(2018, 1, 1), Option.Some(new DateTime(2018,2,1)))
            });
        }

        [Fact]
        public void BindModelAsyncShouldSetModelWithTitleAndContentEmptyWhenNoValuesExists()
        {
            var binder = GetEditPostModelBinder();

            var context = GetContext();
            context.ValueProvider = Substitute.For<IValueProvider>();
            context.ValueProvider.GetValue("Created").Returns(new ValueProviderResult(new StringValues("1/1/2018")));

            binder.BindModelAsync(context).GetAwaiter().OnCompleted(() =>
            {
                context.Result.Model.Should().BeEquivalentTo(new { Title = string.Empty, Content = string.Empty });
            });
        }

        [Fact]
        public void BindModelAsyncShouldThrowArgumentNullExceptionWhenContextIsNull()
        {
            var binder = GetEditPostModelBinder();
            Action bindModelAsync = () => binder.BindModelAsync(null);
            bindModelAsync.Should().Throw<ArgumentNullException>()
                .WithMessage("Value cannot be null.\r\nParameter name: bindingContext").Which.ParamName.Should()
                .BeEquivalentTo("bindingContext");
        }

        [Fact]
        public async void BindModelAsyncShouldSetModelStateErrorWhenNoCreatedDateExists()
        {
            var binder = GetEditPostModelBinder();

            var context = GetContext();

            await binder.BindModelAsync(context);

            context.ModelState["NoCreatedDate"].Errors[0].ErrorMessage.Should().BeEquivalentTo("No created date found");
        }

        [Fact]
        public async void BindModelAsyncShouldSetModelStateErrorWhenCreatedDateIsNotValidExists()
        {
            var binder = GetEditPostModelBinder();

            var context = GetContext();

            context.ValueProvider = Substitute.For<IValueProvider>();
            context.ValueProvider.GetValue("Created").Returns(new ValueProviderResult(new StringValues("invalid")));

            await binder.BindModelAsync(context);

            context.ModelState["NoCreatedDate"].Errors[0].ErrorMessage.Should().BeEquivalentTo("No created date found");
        }
    }
}
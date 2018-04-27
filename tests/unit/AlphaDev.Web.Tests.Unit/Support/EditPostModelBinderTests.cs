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
        private static PrefixModelBindingContext GetContext(IValueProvider valueProvider)
        {
            var context = new DefaultModelBindingContext
            {
                ModelState = new ModelStateDictionary(),
                ValueProvider = valueProvider ?? Substitute.For<IValueProvider>()
            };

            return new PrefixModelBindingContext(context, default);
        }

        private EditPostModelBinderMock GetEditPostModelBinder()
        {
            return new EditPostModelBinderMock();
        }

        [Fact]
        public void BindModelShouldSetModelWithTitleAndContent()
        {
            var binder = GetEditPostModelBinder();

            var valueProvider = Substitute.For<IValueProvider>();
            
            valueProvider.GetValue("Title").Returns(new ValueProviderResult(new StringValues("title")));
            valueProvider.GetValue("Content").Returns(new ValueProviderResult(new StringValues("content")));
            valueProvider.GetValue("Created").Returns(new ValueProviderResult(new StringValues("1/1/2018")));

            var context = GetContext(valueProvider);

            binder.BindModelMock(context);
            context.Result.Model.Should().BeEquivalentTo(new { Title = "title", Content = "content" }, options => options.ExcludingMissingMembers());
        }

        [Fact]
        public void BindModelShouldSetModelWithoutModificationDateWhenItIsMissing()
        {
            var binder = GetEditPostModelBinder();

            var valueProvider = Substitute.For<IValueProvider>();
            valueProvider.GetValue("Created").Returns(new ValueProviderResult(new StringValues("1/1/2018")));

            var context = GetContext(valueProvider);

            binder.BindModelMock(context);

            context.Result.Model.Should().BeEquivalentTo(new
            {
                Dates = new DatesViewModel(new DateTime(2018, 1, 1), default)
            });
        }

        [Fact]
        public void BindModelShouldSetModelWithModificationDateWhenItExists()
        {
            var binder = GetEditPostModelBinder();

            var valueProvider = Substitute.For<IValueProvider>();
            valueProvider.GetValue("Created").Returns(new ValueProviderResult(new StringValues("1/1/2018")));
            valueProvider.GetValue("Modified").Returns(new ValueProviderResult(new StringValues("2/1/2018")));

            var context = GetContext(valueProvider);

            binder.BindModelMock(context);
            context.Result.Model.Should().BeEquivalentTo(new
            {
                Dates = new DatesViewModel(new DateTime(2018, 1, 1), Option.Some(new DateTime(2018,2,1)))
            });
        }

        [Fact]
        public void BindModelShouldSetModelWithTitleAndContentEmptyWhenNoValuesExists()
        {
            var binder = GetEditPostModelBinder();

            var valueProvider = Substitute.For<IValueProvider>();
            valueProvider.GetValue("Created").Returns(new ValueProviderResult(new StringValues("1/1/2018")));

            var context = GetContext(valueProvider);

            binder.BindModelMock(context);
            context.Result.Model.Should().BeEquivalentTo(new { Title = string.Empty, Content = string.Empty });
        }

        [Fact]
        public void BindModelShouldSetModelStateErrorWhenNoCreatedDateExists()
        {
            var binder = GetEditPostModelBinder();

            var context = GetContext(default);

            binder.BindModelMock(context);

            context.ModelState["NoCreatedDate"].Errors[0].ErrorMessage.Should().BeEquivalentTo("No created date found");
        }

        [Fact]
        public void BindModelShouldSetModelStateErrorWhenCreatedDateIsNotValidExists()
        {
            var binder = GetEditPostModelBinder();

            var valueProvider = Substitute.For<IValueProvider>();
            valueProvider.GetValue("Created").Returns(new ValueProviderResult(new StringValues("invalid")));

            var context = GetContext(valueProvider);

            binder.BindModelMock(context);

            context.ModelState["NoCreatedDate"].Errors[0].ErrorMessage.Should().BeEquivalentTo("No created date found");
        }

        public class EditPostModelBinderMock : EditPostModelBinder
        {
            public void BindModelMock(PrefixModelBindingContext context)
            {
                BindModel(context);
            }
        }
    }
}
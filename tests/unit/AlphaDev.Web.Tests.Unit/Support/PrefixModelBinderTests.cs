using System;
using System.Threading.Tasks;
using AlphaDev.Web.Support;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Primitives;
using NSubstitute;
using Xunit;

namespace AlphaDev.Web.Tests.Unit.Support
{
    public class PrefixModelBinderTests
    {
        private static DefaultModelBindingContext GetContext()
        {
            return new DefaultModelBindingContext
            {
                ModelState = new ModelStateDictionary()
            };
        }

        private MockPrefixModelBinder GetPrefixModelBinder()
        {
            return Substitute.ForPartsOf<MockPrefixModelBinder>();
        }

        public abstract class MockPrefixModelBinder : PrefixModelBinder
        {
            protected override void BindModel(PrefixModelBindingContext context)
            {
                BindModelMock(context);
            }

            public abstract void BindModelMock(PrefixModelBindingContext context);
        }

        [Fact]
        public void BindModelAsyncShouldCallBindModelWithoutPrefix()
        {
            var binder = GetPrefixModelBinder();

            var context = GetContext();
            context.ValueProvider = Substitute.For<IValueProvider>();
            context.ValueProvider.GetValue("test").Returns(new ValueProviderResult(new StringValues("test")));

            binder.BindModelAsync(context);
            binder.Received(1).BindModelMock(Arg.Is<PrefixModelBindingContext>(bindingContext =>
                bindingContext.GetValue("test").FirstValue == "test"));
        }

        [Fact]
        public void BindModelAsyncShouldCallBindModelWithPrefix()
        {
            var binder = GetPrefixModelBinder();

            var context = GetContext();
            context.ValueProvider = Substitute.For<IValueProvider>();
            context.ValueProvider.GetValue("prefix").Returns(new ValueProviderResult(new StringValues("prefix")));
            context.ValueProvider.GetValue("prefix.test").Returns(new ValueProviderResult(new StringValues("test")));

            binder.BindModelAsync(context);
            binder.Received(1).BindModelMock(Arg.Is<PrefixModelBindingContext>(bindingContext =>
                bindingContext.GetValue("test").FirstValue == "test"));
        }

        [Fact]
        public void BindModelAsyncShouldReturnCompletedTask()
        {
            var binder = GetPrefixModelBinder();

            var context = GetContext();
            binder.BindModelAsync(context).Should().Be(Task.CompletedTask);
        }

        [Fact]
        public void BindModelAsyncShouldThrowArgumentNullExceptionWhenBindingContextIsNull()
        {
            var binder = GetPrefixModelBinder();

            Action bindModelAsync = () => binder.BindModelAsync(null);

            bindModelAsync.Should()
                .Throw<ArgumentNullException>()
                .Which.ParamName.Should().BeEquivalentTo("bindingContext");
        }
    }
}
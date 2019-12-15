using System;
using System.Threading.Tasks;
using AlphaDev.Web.Core.Support;
using FluentAssertions;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Primitives;
using NSubstitute;
using Xunit;

namespace AlphaDev.Web.Core.Tests.Unit.Support
{
    public class PrefixModelBinderTests
    {
        [NotNull]
        private static DefaultModelBindingContext GetContext() => new DefaultModelBindingContext
        {
            ModelState = new ModelStateDictionary()
        };

        private MockPrefixModelBinder GetPrefixModelBinder() => Substitute.ForPartsOf<MockPrefixModelBinder>();

        public abstract class MockPrefixModelBinder : PrefixModelBinder
        {
            protected override void BindModel(ModelBindingContext context)
            {
                BindModelMock(context);
            }

            public abstract void BindModelMock(ModelBindingContext context);
        }

        [Fact]
        public void BindModelAsyncShouldCallBindModelWithoutPrefix()
        {
            var binder = GetPrefixModelBinder();

            var context = GetContext();
            context.ValueProvider = Substitute.For<IValueProvider>();
            context.ValueProvider.GetValue("test").Returns(new ValueProviderResult(new StringValues("test")));

            binder.BindModelAsync(context);
            binder.Received(1)
                  .BindModelMock(Arg.Is<ModelBindingContext>(bindingContext =>
                      bindingContext.ValueProvider.GetValue("test").FirstValue == "test"));
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
            binder.Received(1)
                  .BindModelMock(Arg.Is<ModelBindingContext>(bindingContext =>
                      bindingContext.ValueProvider.GetValue("test").FirstValue == "test"));
        }

        [Fact]
        public void BindModelAsyncShouldReturnCompletedTask()
        {
            var binder = GetPrefixModelBinder();

            var context = GetContext();
            context.ValueProvider = Substitute.For<IValueProvider>();
            binder.BindModelAsync(context).Should().Be(Task.CompletedTask);
        }

        [Fact]
        public void BindModelAsyncShouldThrowArgumentExceptionWhenValueProviderIsNull()
        {
            var binder = GetPrefixModelBinder();

            Action bindModelAsync = () => binder.BindModelAsync(GetContext());
            bindModelAsync.Should()
                          .Throw<ArgumentException>()
                          .WithMessage("ValueProvider cannot be null. (Parameter 'bindingContext')")
                          .Which
                          .ParamName.Should()
                          .BeEquivalentTo("bindingContext");
        }

        [Fact]
        public void BindModelAsyncShouldThrowArgumentNullExceptionWhenBindingContextIsNull()
        {
            var binder = GetPrefixModelBinder();

            Action bindModelAsync = () => binder.BindModelAsync(null);

            bindModelAsync.Should()
                          .Throw<ArgumentNullException>()
                          .Which.ParamName.Should()
                          .BeEquivalentTo("bindingContext");
        }
    }
}
using System;
using AlphaDev.Web.Support;
using FluentAssertions;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Primitives;
using NSubstitute;
using Xunit;

namespace AlphaDev.Web.Tests.Unit.Support
{
    public class PrefixModelBindingContextTests
    {
        [NotNull]
        private PrefixModelBindingContext GetPrefixModelBindingContext()
        {
            return GetPrefixModelBindingContext(GetContext(), default);
        }

        [NotNull]
        private static DefaultModelBindingContext GetContext()
        {
            return new DefaultModelBindingContext
            {
                ModelState = new ModelStateDictionary()
            };
        }

        [NotNull]
        private PrefixModelBindingContext GetPrefixModelBindingContext([NotNull] ModelBindingContext context,
            string prefix)
        {
            return new PrefixModelBindingContext(context, prefix);
        }

        [Fact]
        public void ConstructorShouldThrowArgumentNullExceptionWhenBindingContextIsNull()
        {
            // ReSharper disable once ObjectCreationAsStatement - just testing constructor
            Action constructor = () => new PrefixModelBindingContext(null, default);

            constructor.Should()
                .Throw<ArgumentNullException>()
                .Which.ParamName.Should()
                .BeEquivalentTo("bindingContext");
        }

        [Fact]
        public void GetValueShouldReturnValueWhenThereIsNoPrefix()
        {
            var context = GetContext();
            context.ValueProvider = Substitute.For<IValueProvider>();
            context.ValueProvider.GetValue("test").Returns(new ValueProviderResult(new StringValues("test")));

            var binder = GetPrefixModelBindingContext(context, default);

            binder.GetValue("test").FirstValue.Should().BeEquivalentTo("test");
        }

        [Fact]
        public void GetValueShouldReturnValueWhenThereIsPrefix()
        {
            var context = GetContext();
            context.ValueProvider = Substitute.For<IValueProvider>();
            context.ValueProvider.GetValue("prefix.test").Returns(new ValueProviderResult(new StringValues("test")));

            var binder = GetPrefixModelBindingContext(context, "prefix.");

            binder.GetValue("test").FirstValue.Should().BeEquivalentTo("test");
        }

        [Fact]
        public void GetValueShouldThrowInvalidOperationExceptionWhenValueProviderIsNull()
        {
            var context = GetContext();
            var binder = GetPrefixModelBindingContext(context, "prefix.");
            Action getValue = () => binder.GetValue(default);

            getValue.Should().Throw<InvalidOperationException>().WithMessage("ValueProvider is null");
        }

        [Fact]
        public void ModelStateShouldReturnModelStateStateDictionary()
        {
            var context = GetContext();
            context.ModelState.AddModelError("test", string.Empty);

            var binder = GetPrefixModelBindingContext(context, default);
            binder.ModelState.ContainsKey("test").Should().BeTrue();
        }

        [Fact]
        public void ResultShouldGetAndSetModelBindingResult()
        {
            var binder = GetPrefixModelBindingContext();
            binder.Result = ModelBindingResult.Success(default);
            binder.Result.Should().Be(ModelBindingResult.Success(default));
        }
    }
}
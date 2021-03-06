﻿using AlphaDev.Web.Core.Support;
using FluentAssertions;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Primitives;
using NSubstitute;
using Xunit;

namespace AlphaDev.Web.Core.Tests.Unit.Support
{
    public class PrefixValueProviderTests
    {
        [NotNull]
        private PrefixValueProvider GetPrefixValueProvider([NotNull] IValueProvider provider, string prefix) =>
            new PrefixValueProvider(provider, prefix);

        [Fact]
        public void ContainsPrefixShouldReturnTrue()
        {
            GetPrefixValueProvider(Substitute.For<IValueProvider>(), string.Empty)
                .ContainsPrefix(default)
                .Should()
                .BeTrue();
        }

        [Fact]
        public void GetValueShouldReturnValueFromValueProviderDependency()
        {
            var dependencyProvider = Substitute.For<IValueProvider>();
            dependencyProvider.GetValue("test").Returns(new ValueProviderResult(new StringValues("value")));
            var provider = GetPrefixValueProvider(dependencyProvider, string.Empty);
            provider.GetValue("test").FirstValue.Should().BeEquivalentTo("value");
        }

        [Fact]
        public void GetValueShouldReturnValueWithPrefixDependency()
        {
            var dependencyProvider = Substitute.For<IValueProvider>();
            dependencyProvider.GetValue("prefixtest").Returns(new ValueProviderResult(new StringValues("value")));
            var provider = GetPrefixValueProvider(dependencyProvider, "prefix");
            provider.GetValue("test").FirstValue.Should().BeEquivalentTo("value");
        }
    }
}
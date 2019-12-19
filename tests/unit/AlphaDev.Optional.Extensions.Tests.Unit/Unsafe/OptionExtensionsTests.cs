﻿using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using AlphaDev.Optional.Extensions.Unsafe;
using FluentAssertions;
using JetBrains.Annotations;
using Optional;
using Optional.Unsafe;
using Xunit;

namespace AlphaDev.Optional.Extensions.Tests.Unit.Unsafe
{
    public static class OptionExtensionsTests
    {
        [Fact]
        public static void ExceptionOrFailureReturnsExceptionWhenOptionHasNone()
        {
            Option.None<string, string>("ex").ExceptionOrFailure().Should().Be("ex");
        }

        [Fact]
        public static void ExceptionOrFailureThrowsInvalidOperationExceptionWhenOptionHasSome()
        {
            Action exceptionOrFailure = () => Option.Some<string?, string>(default).ExceptionOrFailure();
            exceptionOrFailure.Should().Throw<InvalidOperationException>().WithMessage("Exception value is missing.");
        }
    }
}
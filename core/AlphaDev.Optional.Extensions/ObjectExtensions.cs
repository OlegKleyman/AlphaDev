using System;
using Optional;

namespace AlphaDev.Optional.Extensions
{
    public static class ObjectExtensions
    {
        public static Option<T> SomeWhenNotNull<T>(this T? target) where T : class => target.SomeNotNull()!;

        public static Option<T, TException> SomeWhenNotNull<T, TException>(this T? target,
            Func<TException> exceptionFactory) where T : class =>
            target.SomeWhenNotNull().WithException(exceptionFactory);
    }
}
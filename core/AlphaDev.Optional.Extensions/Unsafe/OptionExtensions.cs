using System;
using Optional;

namespace AlphaDev.Optional.Extensions.Unsafe
{
    public static class OptionExtensions
    {
        public static TException ExceptionOrFailure<T, TException>(this Option<T, TException> option)
        {
            return option.Match(_ => throw new InvalidOperationException("Exception value is missing."),
                exception => exception);
        }
    }
}
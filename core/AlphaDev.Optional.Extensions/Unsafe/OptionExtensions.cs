using System;
using System.Reflection;
using Optional;

namespace AlphaDev.Optional.Extensions.Unsafe
{
    public static class OptionExtensions
    {
        public static TException ExceptionOrFailure<T, TException>(this Option<T, TException> option)
        {
            if (option.HasValue)
            {
                throw new InvalidOperationException("Exception value is missing.");
            }

            // Option either always has Exception property. The "?" check is only a formality
            // null return is acceptable if TException happens to be nullable
            return (TException)option.GetType().GetProperty("Exception", BindingFlags.NonPublic | BindingFlags.Instance)?.GetValue(option)!;
        }
    }
}

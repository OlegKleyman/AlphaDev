using System;
using Optional;

namespace AlphaDev.Core.Extensions
{
    public static class OptionExtensions
    {
        public static Option<T, TE> MapToAction<T, TE>(this Option<T, TE> target, Action<T> action)
        {
            target.MatchSome(obj => action(obj));
            return target;
        }

        public static Option<T> MapToAction<T>(this Option<T> target, Action<T> action)
        {
            target.MatchSome(action);
            return target;
        }

        public static Option<T> MapToAction<T>(this Option<T> target, Action action)
        {
            if (target.HasValue) action();

            return target;
        }

        public static Option<T, TE> MapToAction<T, TE>(this Option<T, TE> target, Action action)
        {
            if (target.HasValue) action();

            return target;
        }
    }
}
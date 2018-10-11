using System;
using JetBrains.Annotations;
using Optional;

namespace AlphaDev.Core.Extensions
{
    public static class OptionExtensions
    {
        public static Option<T, TE> MapToAction<T, TE>(this Option<T, TE> target, [NotNull] Action<T> action)
        {
            target.MatchSome(action);
            return target;
        }

        public static Option<T> MapToAction<T>(this Option<T> target, [NotNull] Action<T> action)
        {
            target.MatchSome(action);
            return target;
        }

        public static Option<T> MapToAction<T>(this Option<T> target, Action action)
        {
            target.MatchSome(obj => action());
            return target;
        }

        public static Option<T, TE> MapToAction<T, TE>(this Option<T, TE> target, Action action)
        {
            target.MatchSome(obj => action());
            return target;
        }
    }
}
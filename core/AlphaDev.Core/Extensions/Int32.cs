using JetBrains.Annotations;

namespace AlphaDev.Core.Extensions
{
    public static class Int32
    {
        [NotNull]
        public static PositiveInteger ToPositiveInteger(this int target)
        {
            return new PositiveInteger(target);
        }
    }
}

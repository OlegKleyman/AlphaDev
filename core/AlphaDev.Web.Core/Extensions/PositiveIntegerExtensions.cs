using AlphaDev.Core;
using JetBrains.Annotations;

namespace AlphaDev.Web.Core.Extensions
{
    public static class PositiveIntegerExtensions
    {
        [NotNull]
        public static PositiveInteger ToStartPosition([NotNull] this PositiveInteger page,
            [NotNull] PositiveInteger itemCount) => new PositiveInteger((page.Value - 1) * itemCount.Value + 1);
    }
}
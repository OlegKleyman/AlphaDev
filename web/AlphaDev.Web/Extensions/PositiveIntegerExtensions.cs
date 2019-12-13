using JetBrains.Annotations;

namespace AlphaDev.Web.Extensions
{
    public static class PositiveIntegerExtensions
    {
        [NotNull]
        public static AlphaDev.Core.PositiveInteger ToStartPosition([NotNull] this AlphaDev.Core.PositiveInteger page,
            [NotNull] AlphaDev.Core.PositiveInteger itemCount) =>
            new AlphaDev.Core.PositiveInteger((page.Value - 1) * itemCount.Value + 1);
    }
}
using JetBrains.Annotations;

namespace AlphaDev.Web.Extensions
{
    public static class PositiveInteger
    {
        [NotNull]
        public static Core.PositiveInteger ToStartPosition([NotNull] this Core.PositiveInteger page,
            [NotNull] Core.PositiveInteger itemCount)
        {
            return new Core.PositiveInteger((page.Value - 1) * itemCount.Value + 1);
        }
    }
}
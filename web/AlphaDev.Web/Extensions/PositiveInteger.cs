using JetBrains.Annotations;

namespace AlphaDev.Web.Extensions
{
    public static class PositiveInteger
    {
        [NotNull]
        public static Support.PositiveInteger ToStartPosition([NotNull] this Support.PositiveInteger page, [NotNull] Support.PositiveInteger itemCount)
        {
            return new Support.PositiveInteger((page.Value - 1) * itemCount.Value + 1);
        }
    }
}

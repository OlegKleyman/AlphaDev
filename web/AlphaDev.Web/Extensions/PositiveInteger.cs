namespace AlphaDev.Web.Extensions
{
    public static class PositiveInteger
    {
        public static Support.PositiveInteger ToStartPosition(this Support.PositiveInteger page, Support.PositiveInteger itemCount)
        {
            return new Support.PositiveInteger((page.Value - 1) * itemCount.Value + 1);
        }
    }
}

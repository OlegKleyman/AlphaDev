namespace AlphaDev.Web.Extensions
{
    public static class Int32
    {
        public static Support.PositiveInteger ToPositiveInteger(this int target)
        {
            return new Support.PositiveInteger(target);
        }
    }
}

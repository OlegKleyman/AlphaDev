using System;
using AlphaDev.Core;

namespace AlphaDev.Web.Support
{
    public struct PageBoundaries
    {
        public static readonly PageBoundaries MinValue = new PageBoundaries(PositiveInteger.MinValue, PositiveInteger.MinValue);

        public PageBoundaries(PositiveInteger count, PositiveInteger maxTotal)
        {
            Count = count;
            MaxTotal = maxTotal;
        }

        public PositiveInteger Count { get; }
        public PositiveInteger MaxTotal { get; }

        public int GetTotalPages(int itemCount) => (int)Math.Ceiling(itemCount / (decimal)Count.Value);
    }
}
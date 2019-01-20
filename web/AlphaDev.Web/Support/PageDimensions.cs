using System;

namespace AlphaDev.Web.Support
{
    public struct PageDimensions
    {
        public PositiveInteger Start { get; }
        public PageBoundaries Boundaries { get; }
        public static readonly PageDimensions MinValue = new PageDimensions(PositiveInteger.MinValue, PageBoundaries.MinValue);

        public PageDimensions(PositiveInteger start, PageBoundaries boundaries)
        {
            Start = start;
            Boundaries = boundaries;
        }
    }
}
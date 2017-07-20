using System;
using Optional;

namespace AlphaDev.Core
{
    public abstract class BlogBase
    {
        private static readonly Lazy<EmptyBlog> EmptyBlogEntry = new Lazy<EmptyBlog>(() => new EmptyBlog());

        public abstract string Title { get; }

        public abstract Dates Dates { get; }

        public abstract string Content { get; }

        public static BlogBase Empty => EmptyBlogEntry.Value;

        private class EmptyBlog : BlogBase
        {
            private static readonly Dates EmptyDates = new Dates(default(DateTime), Option.None<DateTime>());

            public override string Title => string.Empty;

            public override Dates Dates => EmptyDates;

            public override string Content => string.Empty;
        }
    }
}
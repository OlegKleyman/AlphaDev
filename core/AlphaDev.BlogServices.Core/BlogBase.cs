using System;
using JetBrains.Annotations;
using Optional;

namespace AlphaDev.BlogServices.Core
{
    public abstract class BlogBase
    {
        private static readonly Lazy<EmptyBlog> EmptyBlogEntry = new Lazy<EmptyBlog>(() => new EmptyBlog());

        public abstract string Title { get; }

        public abstract Dates Dates { get; }

        public abstract string Content { get; }

        public static BlogBase Empty => EmptyBlogEntry.Value;

        public abstract int Id { get; }

        private class EmptyBlog : BlogBase
        {
            private static readonly Dates EmptyDates = new Dates(default, Option.None<DateTime>());

            [NotNull]
            public override string Title => string.Empty;

            public override Dates Dates => EmptyDates;

            [NotNull]
            public override string Content => string.Empty;

            public override int Id => default;
        }
    }
}
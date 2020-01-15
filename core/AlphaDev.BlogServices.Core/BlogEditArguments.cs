using System;
using JetBrains.Annotations;

namespace AlphaDev.BlogServices.Core
{
    public class BlogEditArguments
    {
        private string? _content;

        private string? _title;

        [NotNull]
        public string Content
        {
            get => _content ?? throw new InvalidOperationException($"{nameof(Content)} is not initialized.");
            set => _content = value;
        }

        [NotNull]
        public string Title
        {
            get => _title ?? throw new InvalidOperationException($"{nameof(Title)} is not initialized.");
            set => _title = value;
        }
    }
}
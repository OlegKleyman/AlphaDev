using System;
using JetBrains.Annotations;

namespace AlphaDev.Core
{
    public class BlogEditArguments
    {
        private string? _content;

        [NotNull]
        public string Content
        {
            get => _content ?? throw new InvalidOperationException($"{nameof(Content)} is not initialized.");
            set => _content = value;
        }

        private string? _title;

        [NotNull]
        public string Title
        {
            get => _title ?? throw new InvalidOperationException($"{nameof(Title)} is not initialized.");
            set => _title = value;
        }
    }
}
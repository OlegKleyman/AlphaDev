using System;
using JetBrains.Annotations;

namespace AlphaDev.Core.Data.Entities
{
    public class Blog
    {
        public int Id { get; set; }

        public DateTime Created { get; set; }

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

        public DateTime? Modified { get; set; }
    }
}
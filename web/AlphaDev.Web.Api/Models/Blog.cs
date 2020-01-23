using System;
using JetBrains.Annotations;

namespace AlphaDev.Web.Api.Models
{
    public class Blog
    {
        public int Id { get; set; }

        private string? _title;

        public string Title
        {
            get => _title ?? throw new InvalidOperationException($"{nameof(Title)} is missing");
            set => _title = value;
        }

        private string? _content;

        public string Content
        {
            get => _content ?? throw new InvalidOperationException($"{nameof(Content)} is missing.");
            set => _content = value;
        }

        private Dates? _dates;

        [NotNull]
        public Dates Dates
        {
            get => _dates ?? throw new InvalidOperationException($"{nameof(Dates)} is missing.");
            set => _dates = value;
        }
    }
}
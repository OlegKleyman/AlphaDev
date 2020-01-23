﻿using System;

namespace AlphaDev.Services.Web.Models
{
    public class Blog
    {
        public Blog(Dates dates) => Dates = dates;

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

        public Dates Dates { get; }
    }
}
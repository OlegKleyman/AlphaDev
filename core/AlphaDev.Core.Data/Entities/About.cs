using System;
using JetBrains.Annotations;

namespace AlphaDev.Core.Data.Entities
{
    public class About
    {
        private string? _value;

        [NotNull]
        public string Value
        {
            get => _value ?? throw new InvalidOperationException($"{nameof(Value)} is not initialized.");
            set => _value = value;
        }

        public bool Id { get; set; }
    }
}
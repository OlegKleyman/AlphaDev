﻿using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace AlphaDev.Web.Core.Support
{
    public class PrefixValueProvider : IValueProvider
    {
        private readonly string _prefix;
        private readonly IValueProvider _valueProvider;

        public PrefixValueProvider([NotNull] IValueProvider valueProvider, string prefix)
        {
            _valueProvider = valueProvider;
            _prefix = prefix;
        }

        public bool ContainsPrefix(string? prefix) => true;

        public ValueProviderResult GetValue(string key) => _valueProvider.GetValue(_prefix + key);
    }
}
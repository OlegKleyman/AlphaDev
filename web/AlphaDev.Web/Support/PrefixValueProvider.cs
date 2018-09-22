﻿using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace AlphaDev.Web.Support
{
    public class PrefixValueProvider : IValueProvider
    {
        private readonly IValueProvider _valueProvider;
        private readonly string _prefix;

        public PrefixValueProvider([NotNull] IValueProvider valueProvider, string prefix)
        {
            _valueProvider = valueProvider;
            _prefix = prefix;
        }

        public bool ContainsPrefix(string prefix)
        {
            return true;
        }

        public ValueProviderResult GetValue(string key)
        {
            return _valueProvider.GetValue(_prefix + key);
        }
    }
}
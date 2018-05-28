using System;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace AlphaDev.Web.Support
{
    public class PrefixModelBindingContext
    {
        private readonly ModelBindingContext _bindingContext;
        private readonly string _prefix;

        public PrefixModelBindingContext([CanBeNull] ModelBindingContext bindingContext, string prefix)
        {
            _bindingContext = bindingContext ?? throw new ArgumentNullException(nameof(bindingContext));
            _prefix = prefix;
        }

        public ModelStateDictionary ModelState => _bindingContext.ModelState;

        public ModelBindingResult Result
        {
            get => _bindingContext.Result;
            set => _bindingContext.Result = value;
        }

        public ValueProviderResult GetValue(string key)
        {
            return (_bindingContext.ValueProvider ?? throw new InvalidOperationException("ValueProvider is null"))
                .GetValue(_prefix + key);
        }
    }
}
using System;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Optional;

namespace AlphaDev.Web.Support
{
    public abstract class PrefixModelBinder : IModelBinder
    {
        public Task BindModelAsync([CanBeNull] ModelBindingContext bindingContext)
        {
            if (bindingContext is null) throw new ArgumentNullException(nameof(bindingContext));
            if (bindingContext.ValueProvider is null)
            {
                throw new ArgumentException("ValueProvider cannot be null.", nameof(bindingContext));
            }

            var prefix = bindingContext.ValueProvider.GetValue("prefix").FirstValue
                .SomeWhen(s => !string.IsNullOrWhiteSpace(s))
                .Map(s => s + ".")
                .ValueOr(string.Empty);
            bindingContext.ValueProvider = new PrefixValueProvider(bindingContext.ValueProvider, prefix);
            BindModel(bindingContext);
            return Task.CompletedTask;
        }

        protected abstract void BindModel(ModelBindingContext context);
    }
}
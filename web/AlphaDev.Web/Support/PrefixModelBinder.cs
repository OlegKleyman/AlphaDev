using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Optional;

namespace AlphaDev.Web.Support
{
    public abstract class PrefixModelBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            if (bindingContext == null) throw new ArgumentNullException(nameof(bindingContext));

            var prefix = (bindingContext.ValueProvider?.GetValue("prefix").FirstValue)
                .SomeWhen(s => !string.IsNullOrWhiteSpace(s))
                .Map(s => s + ".")
                .ValueOr(string.Empty);

            var prefixModelBindingContext = new PrefixModelBindingContext(bindingContext, prefix);
            BindModel(prefixModelBindingContext);
            return Task.CompletedTask;
        }

        protected abstract void BindModel(PrefixModelBindingContext context);
    }
}
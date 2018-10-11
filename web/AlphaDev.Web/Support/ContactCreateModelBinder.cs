using AlphaDev.Web.Models;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace AlphaDev.Web.Support
{
    public class ContactCreateModelBinder : PrefixModelBinder
    {
        protected override void BindModel([NotNull] ModelBindingContext context)
        {
            var value = context.ValueProvider.GetValue("Value").FirstValue ?? string.Empty;

            var model = new ContactCreateViewModel(value);

            context.Result = ModelBindingResult.Success(model);
        }
    }
}
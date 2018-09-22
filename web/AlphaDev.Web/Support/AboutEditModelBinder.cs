using System;
using AlphaDev.Web.Models;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Optional;

namespace AlphaDev.Web.Support
{
    public class AboutEditModelBinder : PrefixModelBinder
    {
        protected override void BindModel([NotNull] ModelBindingContext context)
        {
            var value = context.ValueProvider.GetValue("Value").FirstValue ?? string.Empty;

            var model = new AboutEditorViewModel(value);

            context.Result = ModelBindingResult.Success(model);
        }
    }
}
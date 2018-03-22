using System;
using System.Threading.Tasks;
using AlphaDev.Web.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace AlphaDev.Web.Support
{
    public class CreatePostModelBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            if (bindingContext == null) throw new ArgumentNullException(nameof(bindingContext));

            var title = bindingContext.ValueProvider?.GetValue("Title").FirstValue;
            var content = bindingContext.ValueProvider?.GetValue("Content").FirstValue;

            var model = new CreatePostViewModel(title ?? string.Empty, content ?? string.Empty);

            bindingContext.Result = ModelBindingResult.Success(model);

            return Task.CompletedTask;
        }
    }
}
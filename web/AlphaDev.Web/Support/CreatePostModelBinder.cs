using AlphaDev.Web.Models;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace AlphaDev.Web.Support
{
    public class CreatePostModelBinder : PrefixModelBinder
    {
        protected override void BindModel([NotNull] ModelBindingContext context)
        {
            var title = context.ValueProvider.GetValue("Title").FirstValue;
            var content = context.ValueProvider.GetValue("Content").FirstValue;

            var model = new CreatePostViewModel(title ?? string.Empty, content ?? string.Empty);

            context.Result = ModelBindingResult.Success(model);
        }
    }
}
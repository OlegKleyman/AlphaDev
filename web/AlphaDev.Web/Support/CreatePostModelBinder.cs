using System;
using System.Threading.Tasks;
using AlphaDev.Web.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace AlphaDev.Web.Support
{
    public class CreatePostModelBinder : PrefixModelBinder
    {
        protected override void BindModel(PrefixModelBindingContext context)
        {
            var title = context.GetValue("Title").FirstValue;
            var content = context.GetValue("Content").FirstValue;

            var model = new CreatePostViewModel(title ?? string.Empty, content ?? string.Empty);

            context.Result = ModelBindingResult.Success(model);
        }
    }
}
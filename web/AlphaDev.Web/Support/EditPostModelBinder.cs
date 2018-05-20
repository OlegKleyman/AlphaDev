using System;
using AlphaDev.Web.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Optional;

namespace AlphaDev.Web.Support
{
    public class EditPostModelBinder : PrefixModelBinder
    {
        protected override void BindModel(PrefixModelBindingContext context)
        {
            if (!DateTime.TryParse(context.GetValue("Created").FirstValue, out DateTime created))
            {
                context.ModelState.AddModelError("NoCreatedDate", "No created date found");
                context.Result = ModelBindingResult.Failed();
            }
            else
            {
                var title = context.GetValue("Title").FirstValue;
                var content = context.GetValue("Content").FirstValue;

                var modified = DateTime
                    .TryParse(context.GetValue("Modified").FirstValue, out DateTime modDate)
                    .SomeWhen(isValidDate => isValidDate)
                    .Map(b => modDate);

                var model = new EditPostViewModel(title ?? string.Empty, content ?? string.Empty,
                    new DatesViewModel(created, modified));

                context.Result = ModelBindingResult.Success(model);
            }
        }
    }
}
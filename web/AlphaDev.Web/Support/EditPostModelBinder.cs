using System;
using System.Threading.Tasks;
using AlphaDev.Web.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Omego.Extensions.OptionExtensions;
using Optional;

namespace AlphaDev.Web.Support
{
    public class EditPostModelBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            if (bindingContext == null) throw new ArgumentNullException(nameof(bindingContext));

            if (!DateTime.TryParse(bindingContext.ValueProvider?.GetValue("Created").FirstValue, out DateTime created))
            {
                bindingContext.ModelState.AddModelError("NoCreatedDate", "No created date found");
                bindingContext.Result = ModelBindingResult.Failed();
            }
            else
            {
                var title = bindingContext.ValueProvider?.GetValue("Title").FirstValue;
                var content = bindingContext.ValueProvider?.GetValue("Content").FirstValue;

                var modified = DateTime
                    .TryParse(bindingContext.ValueProvider?.GetValue("Modified").FirstValue, out DateTime modDate)
                    .SomeWhen(isValidDate => isValidDate)
                    .Map(b => modDate);

                var model = new EditPostViewModel(title ?? string.Empty, content ?? string.Empty,
                    new DatesViewModel(created, modified));

                bindingContext.Result = ModelBindingResult.Success(model);
            }

            return Task.CompletedTask;
        }
    }
}
using System;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace AlphaDev.Web.TagHelpers
{
    public class DisplayTagHelper : TagHelper
    {
        public bool Value { get; set; } = true;

        public override Task ProcessAsync(TagHelperContext context, [CanBeNull] TagHelperOutput output)
        {
            return Task.Run(() => Process(context, output));
        }

        public override void Process(TagHelperContext context, [CanBeNull] TagHelperOutput output)
        {
            if (output == null) throw new ArgumentNullException(nameof(output));

            output.TagName = string.Empty;

            if (!Value) output.SuppressOutput();

            base.Process(context, output);
        }
    }
}
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace AlphaDev.Web.TagHelpers
{
    public class DisplayTagHelper : TagHelper
    {
        public bool Value { get; set; } = true;

        public override Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            return Task.Run(
                () => Process(context, output));
        }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (output == null) throw new ArgumentNullException(nameof(output));

            output.TagName = string.Empty;

            if (!Value) output.SuppressOutput();

            base.Process(context, output);
        }
    }
}
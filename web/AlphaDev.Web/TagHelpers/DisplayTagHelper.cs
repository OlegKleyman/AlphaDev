using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlphaDev.Web.TagHelpers
{
    using Microsoft.AspNetCore.Razor.TagHelpers;

    public class DisplayTagHelper : TagHelper
    {
        public bool Value { get; set; } = true;

        public override Task ProcessAsync(TagHelperContext context, TagHelperOutput output) => Task.Run(
            () => Process(context, output));

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "div";
            output.Attributes.Add(new TagHelperAttribute("style", Value ? "display: inline;" : "display: none;"));

            base.Process(context, output);
        }
    }
}

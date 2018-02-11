using System;
using System.Threading.Tasks;
using Markdig;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace AlphaDev.Web.TagHelpers
{
    public class MarkdownTagHelper : TagHelper
    {
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var content = (output ?? throw new ArgumentNullException(nameof(output)))
                .GetChildContentAsync(NullHtmlEncoder.Default)
                .GetAwaiter().GetResult();

            output.Content.SetHtmlContent(Markdown.ToHtml(content.GetContent()));

            output.TagName = string.Empty;

            base.Process(context, output);
        }

        public override Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            return Task.Run(() => Process(context, output));
        }
    }
}
using System;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Markdig;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace AlphaDev.Web.TagHelpers
{
    public class MarkdownTagHelper : TagHelper
    {
        public override void Process(TagHelperContext context, [CanBeNull] TagHelperOutput output)
        {
            var content = (output ?? throw new ArgumentNullException(nameof(output)))
                          .GetChildContentAsync(NullHtmlEncoder.Default)
                          ?.GetAwaiter()
                          .GetResult();

            output.Content.SetHtmlContent(Markdown.ToHtml(content?.GetContent()?.Trim()));

            output.TagName = string.Empty;

            base.Process(context, output);
        }

        public override Task ProcessAsync(TagHelperContext context, [CanBeNull] TagHelperOutput output)
        {
            return Task.Run(() => Process(context, output));
        }
    }
}
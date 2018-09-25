using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace AlphaDev.Web.TagHelpers
{
    public class ScriptsTagHelper : TagHelper
    {
        [ViewContext] public ViewContext Context { get; set; }

        public override void Process(TagHelperContext context, [NotNull] TagHelperOutput output)
        {
            var scripts = (HashSet<string>) (Context?.ViewData["AllScripts"] ?? new HashSet<string>());

            output.TagName = string.Empty;
            output.Content.AppendHtml(string.Join('\n', scripts.Select(s => $"<script src=\"{s}\"></script>")));

            var inlineScripts = Context?.ViewData["InlineScripts"] ?? string.Empty;

            output.Content.AppendHtml(inlineScripts.ToString());
        }
    }
}
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Optional;

namespace AlphaDev.Web.TagHelpers
{
    public class LinksTagHelper : TagHelper
    {
        [ViewContext] public ViewContext Context { [CanBeNull] get; [NotNull] set; }

        public override void Process(TagHelperContext context, [NotNull] TagHelperOutput output)
        {
            var links = (HashSet<string>) (Context?.ViewData["AllLinks"] ?? new HashSet<string>());
            var inlineStyles = (HashSet<string>) (Context?.ViewData["InlineStyles"] ?? new HashSet<string>());

            output.TagName = string.Empty;
            output.Content.AppendHtml(string.Join('\n',
                links.Select(s => $"<link href=\"{s}\" rel=\"stylesheet\" />")));

            string.Join('\n', inlineStyles)
                .SomeWhen(s => !string.IsNullOrWhiteSpace(s))
                .Map(s => $"<style>{s}</style>")
                .MatchSome(s => output.Content.AppendHtml(s));
        }
    }
}
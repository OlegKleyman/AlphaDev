using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Markdig;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Optional;

namespace AlphaDev.Web.TagHelpers
{
    public class LinksTagHelper : TagHelper
    {
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var links = (HashSet<string>)(Context?.ViewData["AllLinks"] ?? new HashSet<string>());
            var inlineStyles = (HashSet<string>)(Context?.ViewData["InlineStyles"] ?? new HashSet<string>());

            output.TagName = string.Empty;
            output.Content.AppendHtml(string.Join('\n', links.Select(s => $"<link href=\"{s}\" rel=\"stylesheet\" />")));

            string.Join('\n', inlineStyles)
                .SomeWhen(s => !string.IsNullOrWhiteSpace(s))
                .Map(s => $"<style>{s}</style>")
                .MatchSome(s => output.Content.AppendHtml(s));
        }

        [ViewContext]
        public ViewContext Context { get; set; }

        public override Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            return Task.Run(() => Process(context, output));
        }
    }
}
using System.Collections.Generic;
using System.Threading.Tasks;
using AlphaDev.Core;
using AlphaDev.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace AlphaDev.Web.TagHelpers
{
    public class BlogEditorTagHelper : TagHelper
    {
        private readonly IHtmlHelper _htmlHelper;
        private readonly IPrefixGenerator _prefixGenerator;
        private readonly IUrlHelperFactory _urlHelperFactory;

        public BlogEditorTagHelper(IHtmlHelper htmlHelper, IUrlHelperFactory urlHelperFactory,
            IPrefixGenerator prefixGenerator)
        {
            _htmlHelper = htmlHelper;
            _urlHelperFactory = urlHelperFactory;
            _prefixGenerator = prefixGenerator;
        }

        public BlogEditorViewModel Model { get; set; }

        [ViewContext] public ViewContext Context { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            ((IViewContextAware) _htmlHelper).Contextualize(Context);

            output.TagName = string.Empty;
            var urlHelper = _urlHelperFactory.GetUrlHelper(Context);
            SetReferencedLinks(urlHelper);
            SetReferencedScripts(urlHelper);

            Context.ViewData.TemplateInfo.HtmlFieldPrefix = _prefixGenerator.Generate();

            SetInlineScripts();

            var content = _htmlHelper.PartialAsync("_BlogEditor", Model, Context.ViewData).GetAwaiter().GetResult();
            output.Content.SetHtmlContent(content);
        }

        private void SetReferencedLinks(IUrlHelper urlHelper)
        {
            var links = (HashSet<string>) (Context.ViewData["AllLinks"] ?? new HashSet<string>());
            links.Add(urlHelper.Content("~/lib/bootstrap-markdown/css/bootstrap-markdown.min.css"));
            Context.ViewData["AllLinks"] = links;
        }

        private void SetInlineScripts()
        {
            Context.ViewData["InlineScripts"] = Context.ViewData["InlineScripts"] + $@"<script type=""text/javascript"">
                                                    $('#{_htmlHelper.Id("Content")}').markdown({{
                                                                savable: true,
                                                        onChange: function() {{
                                                                    Prism.highlightAll();
                                                                }},
                                                        onSave: function() {{
                                                            $('#{_htmlHelper.Id("editForm")}').submit();
                                                                }}
                                                            }})
                                                </script>";
        }

        private void SetReferencedScripts(IUrlHelper urlHelper)
        {
            var scripts = (HashSet<string>) (Context.ViewData["AllScripts"] ?? new HashSet<string>());
            scripts.Add(urlHelper.Content("~/lib/marked/marked.min.js"));
            scripts.Add(urlHelper.Content("~/lib/bootstrap-markdown/js/bootstrap-markdown.js"));
            Context.ViewData["AllScripts"] = scripts;
        }

        public override Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            return Task.Run(() => Process(context, output));
        }
    }
}
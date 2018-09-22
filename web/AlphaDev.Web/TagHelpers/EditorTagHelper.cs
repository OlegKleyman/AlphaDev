using System.Collections.Generic;
using System.Threading.Tasks;
using AlphaDev.Core;
using AlphaDev.Web.Support;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace AlphaDev.Web.TagHelpers
{
    public abstract class EditorTagHelper : TagHelper
    {
        private readonly IHtmlHelper _htmlHelper;
        private readonly IUrlHelperFactory _urlHelperFactory;
        private readonly EditorView _editorView;

        [NotNull] [ViewContext] public ViewContext Context { get; set; }
        public object Model { get; set; }

        // ReSharper disable once NotNullMemberIsNotInitialized - initialized implicitly
        protected EditorTagHelper(IHtmlHelper htmlHelper, IUrlHelperFactory urlHelperFactory,
            EditorView editorView)
        {
            _htmlHelper = htmlHelper;
            _urlHelperFactory = urlHelperFactory;
            _editorView = editorView;
        }

        public sealed override void Process(TagHelperContext context, [NotNull] TagHelperOutput output)
        {
            ((IViewContextAware)_htmlHelper).Contextualize(Context);
            Context.ViewData.TemplateInfo.HtmlFieldPrefix = _editorView.Prefix;
            output.TagName = string.Empty;
            var urlHelper = _urlHelperFactory.GetUrlHelper(Context);

            SetReferencedLinks(urlHelper);
            SetReferencedScripts(urlHelper);
            SetInlineScripts();
            
            var content = _htmlHelper.PartialAsync(_editorView.Name, Model, Context.ViewData).GetAwaiter().GetResult();
            output.Content.SetHtmlContent(content);
        }

        private void SetReferencedLinks([NotNull] IUrlHelper urlHelper)
        {
            var links = (HashSet<string>)(Context.ViewData["AllLinks"] ?? new HashSet<string>());
            links.Add(urlHelper.Content("~/lib/bootstrap-markdown/css/bootstrap-markdown.min.css"));
            Context.ViewData["AllLinks"] = links;
        }

        private void SetInlineScripts()
        {
            Context.ViewData["InlineScripts"] = Context.ViewData["InlineScripts"] + $@"<script type=""text/javascript"">
                                                    $('#{_htmlHelper.Id($"{_editorView.EditorElementName}")}').markdown({{
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

        private void SetReferencedScripts([NotNull] IUrlHelper urlHelper)
        {
            var scripts = (HashSet<string>)(Context.ViewData["AllScripts"] ?? new HashSet<string>());
            scripts.Add(urlHelper.Content("~/lib/marked/marked.min.js"));
            scripts.Add(urlHelper.Content("~/lib/bootstrap-markdown/js/bootstrap-markdown.js"));
            Context.ViewData["AllScripts"] = scripts;
        }

        public sealed override Task ProcessAsync(TagHelperContext context, [NotNull] TagHelperOutput output)
        {
            return Task.Run(() => Process(context, output));
        }
    }
}
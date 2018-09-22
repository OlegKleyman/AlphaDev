using System.Collections.Generic;
using System.Threading.Tasks;
using AlphaDev.Core;
using AlphaDev.Web.Models;
using AlphaDev.Web.Support;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace AlphaDev.Web.TagHelpers
{
    public class BlogEditorTagHelper : EditorTagHelper
    {
        public BlogEditorTagHelper(IHtmlHelper htmlHelper, IUrlHelperFactory urlHelperFactory, [NotNull] IPrefixGenerator prefixGenerator) : base(htmlHelper, urlHelperFactory, new EditorView("_BlogEditor", prefixGenerator.Generate(), "Content"))
        {
        }
    }
}
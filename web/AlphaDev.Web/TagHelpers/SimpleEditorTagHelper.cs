using System;
using System.Linq;
using AlphaDev.Core;
using AlphaDev.Web.Models;
using AlphaDev.Web.Support;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;

namespace AlphaDev.Web.TagHelpers
{
    public class SimpleEditorTagHelper : EditorTagHelper
    {
        // ReSharper disable once NotNullMemberIsNotInitialized - Initialized by the framework
        public SimpleEditorTagHelper(IHtmlHelper htmlHelper, IUrlHelperFactory urlHelperFactory,
            [NotNull] IPrefixGenerator prefixGenerator) : base(htmlHelper, urlHelperFactory, new EditorView("_simpleEditor", prefixGenerator.Generate(), "Value"))
        {
        }
    }
}

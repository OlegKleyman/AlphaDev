﻿using AlphaDev.Core;
using AlphaDev.Web.Support;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;

namespace AlphaDev.Web.TagHelpers
{
    public class SimpleEditorTagHelper : EditorTagHelper
    {
        public SimpleEditorTagHelper(IHtmlHelper htmlHelper, IUrlHelperFactory urlHelperFactory,
            [NotNull] IPrefixGenerator prefixGenerator) : base(htmlHelper, urlHelperFactory,
            new EditorView("_simpleEditor", prefixGenerator.Generate(), "Value"))
        {
        }
    }
}
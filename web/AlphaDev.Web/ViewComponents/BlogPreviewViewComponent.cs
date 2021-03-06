﻿using System.Threading.Tasks;
using AlphaDev.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace AlphaDev.Web.ViewComponents
{
    public class BlogPreviewViewComponent : ViewComponent
    {
        public Task<IViewComponentResult> InvokeAsync(BlogViewModel blogViewModel)
        {
            return Task.Run(
                () => (IViewComponentResult) View("BlogPreview", blogViewModel));
        }
    }
}
namespace AlphaDev.Web.Controllers
{
    using System;

    using AlphaDev.Web.Models;

    using AppDev.Core;

    using Microsoft.AspNetCore.Mvc;

    using Optional;

    public class DefaultController : Controller
    {
        private readonly IBlogService blogService;

        public DefaultController(IBlogService blogService) => this.blogService = blogService;

        public ViewResult Index()
        {
            var blog = blogService.GetLatest();
            var model = new BlogViewModel(
                blog.Title,
                blog.Content,
                new DatesViewModel(blog.Dates.Created, blog.Dates.Modified));

            return View(nameof(Index), model);
        }
    }
}
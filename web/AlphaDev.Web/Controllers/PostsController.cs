﻿using System.Linq;
using AlphaDev.Core;
using AlphaDev.Core.Extensions;
using AlphaDev.Optional.Extensions;
using AlphaDev.Web.Core;
using AlphaDev.Web.Core.Extensions;
using AlphaDev.Web.Core.Support;
using AlphaDev.Web.Models;
using AlphaDev.Web.Support;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Optional;

namespace AlphaDev.Web.Controllers
{
    [Route("posts")]
    public class PostsController : Controller
    {
        private readonly IBlogService _blogService;

        public PostsController([NotNull] IBlogService blogService) => _blogService = blogService;

        [Route("page/{page}")]
        public IActionResult Page(int page)
        {
            const int itemsPerPage = 10;
            var maxPagesToDisplay = 10.ToPositiveInteger();
            var startPage = page.ToPositiveInteger();
            var startPosition = startPage.ToStartPosition(itemsPerPage.ToPositiveInteger());
            var blogs = _blogService.GetOrderedByDates(startPosition.Value, itemsPerPage);
            return blogs.Select(blog => new BlogViewModel(blog.Id,
                            blog.Title,
                            blog.Content,
                            new DatesViewModel(blog.Dates.Created, blog.Dates.Modified)))
                        .SomeWhen(x => x.Any(), NotFound())
                        .Match(x => (ActionResult) View("Index", x.ToPager(new PageDimensions(startPage,
                                new PageBoundaries(itemsPerPage.ToPositiveInteger(), maxPagesToDisplay)),
                            _blogService.GetCount(startPosition.Value).ToPositiveInteger())), x => x);
        }

        [Route("{id}")]
        public ActionResult Index(int id)
        {
            var option = _blogService.Get(id)
                                     .WithException(NotFound)
                                     .Map(foundBlog => new BlogViewModel(foundBlog.Id,
                                         foundBlog.Title,
                                         foundBlog.Content,
                                         new DatesViewModel(foundBlog.Dates.Created, foundBlog.Dates.Modified)));
            option.MatchSome(model => ViewBag.Title = model.Title);
            return option.Map<ActionResult>(model => View("Post", model)).GetValueOrException();
        }

        [Authorize]
        [Route("create")]
        public ViewResult Create() => View(nameof(Create));

        [Authorize]
        [SaveFilter]
        [Route("create")]
        [HttpPost]
        public ActionResult Create(CreatePostViewModel? post)
        {
            return post
                   .SomeWhenNotNull()
                   .Filter(x => ModelState.IsValid)
                   .Map(x => _blogService.Add(new Blog(x.Title, x.Content)))
                   .Map(blog => (ActionResult) RedirectToAction(nameof(Index), new { id = blog.Id }))
                   .ValueOr(View(nameof(Create), post));
        }

        [Authorize]
        [SaveFilter]
        [Route("delete/{id}")]
        [HttpPost]
        public IActionResult Delete(int id)
        {
            _blogService.Delete(id);

            return RedirectToAction(nameof(Page), new { page = 1 });
        }

        [Authorize]
        [Route("edit/{id}")]
        public IActionResult Edit(int id)
        {
            return _blogService
                   .Get(id)
                   .Map(b => new EditPostViewModel(b.Title, b.Content,
                       new DatesViewModel(b.Dates.Created, b.Dates.Modified)))
                   .Match(model => (IActionResult) View(nameof(Edit), model), NotFound);
        }

        [Authorize]
        [SaveFilter]
        [Route("edit/{id}")]
        [HttpPost]
        public IActionResult Edit(int id, [CanBeNull] EditPostViewModel model)
        {
            var option = model.SomeWhenNotNull()
                              .Filter(x => ModelState.IsValid)
                              .WithException(() => View(nameof(Edit), model));

            option.MatchSome(x => _blogService.Edit(id, arguments =>
            {
                arguments.Content = x.Content;
                arguments.Title = x.Title;
            }));

            return option
                   .Map(dictionary => (IActionResult) RedirectToAction(nameof(Index), new { id }))
                   .GetValueOrException();
        }
    }
}
using System.Linq;
using AlphaDev.Core;
using AlphaDev.Core.Extensions;
using AlphaDev.Web.Extensions;
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

        public PostsController([NotNull] IBlogService blogService)
        {
            _blogService = blogService;
        }

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
                .Match(x => (ActionResult)View("Index", x.ToPager(new PageDimensions(startPage,
                        new PageBoundaries(itemsPerPage.ToPositiveInteger(), maxPagesToDisplay)),
                    _blogService.GetCount(startPosition.Value).ToPositiveInteger())), x => x);
        }

        [Route("{id}")]
        public ActionResult Index(int id)
        {
            return _blogService.Get(id)
                .Map(foundBlog => new BlogViewModel(foundBlog.Id,
                    foundBlog.Title,
                    foundBlog.Content,
                    new DatesViewModel(foundBlog.Dates.Created, foundBlog.Dates.Modified)))
                .MapToAction(model => ViewBag.Title = model.Title)
                .Map(model => (ActionResult) View("Post", model)).ValueOr(NotFound);
        }

        [Authorize]
        [Route("create")]
        public ViewResult Create()
        {
            return View(nameof(Create));
        }

        [Authorize]
        [Route("create")]
        [HttpPost]
        public ActionResult Create([CanBeNull] CreatePostViewModel post)
        {
            return ModelState
                .SomeWhen(dictionary => dictionary.IsValid)
                .Map(dictionary => _blogService.Add(new Blog(post?.Title, post?.Content)))
                .Map(blog => (ActionResult) RedirectToAction(nameof(Index), new { id = blog.Id }))
                .ValueOr(View(nameof(Create), post));
        }

        [Authorize]
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
        [Route("edit/{id}")]
        [HttpPost]
        public IActionResult Edit(int id, [CanBeNull] EditPostViewModel model)
        {
            return ModelState
                .SomeWhen(dictionary => dictionary.IsValid)
                .MapToAction(dictionary => _blogService.Edit(id, arguments =>
                {
                    arguments.Content = model?.Content;
                    arguments.Title = model?.Title;
                }))
                .Map(dictionary => (IActionResult) RedirectToAction(nameof(Index)))
                .ValueOr(View(nameof(Edit), model));
        }
    }
}
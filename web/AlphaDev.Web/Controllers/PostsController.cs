using System.Linq;
using System.Threading.Tasks;
using AlphaDev.Core;
using AlphaDev.Core.Extensions;
using AlphaDev.Optional.Extensions;
using AlphaDev.Web.Core;
using AlphaDev.Web.Core.Extensions;
using AlphaDev.Web.Core.Support;
using AlphaDev.Web.Models;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Optional;
using Optional.Async;

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
        public async Task<ActionResult> Index(int id)
        {
            var option = _blogService.GetAsync(id)
                                     .WithExceptionAsync(NotFound)
                                     .MapAsync(foundBlog => new BlogViewModel(foundBlog.Id,
                                         foundBlog.Title,
                                         foundBlog.Content,
                                         new DatesViewModel(foundBlog.Dates.Created, foundBlog.Dates.Modified)));
            await option.MatchSomeAsync(model => ViewData["Title"] = model.Title);
            return await option.MapAsync(model => (ActionResult)View("Post", model)).GetValueOrExceptionAsync();
        }

        [Authorize]
        [Route("create")]
        public ViewResult Create() => View(nameof(Create));

        [Authorize]
        [SaveFilter]
        [Route("create")]
        [HttpPost]
        public async Task<ActionResult> Create(CreatePostViewModel? post)
        {
            return await post
                   .SomeWhenNotNull()
                   .Filter(x => ModelState.IsValid)
                   .MapAsync(model => _blogService.AddAsync(new Blog(model.Title, model.Content)))
                   .MapAsync(blog => (ActionResult)RedirectToAction(nameof(Index), new { id = blog.Id }))
                   .WithExceptionAsync(() => View(nameof(Create), post))
                   .GetValueOrExceptionAsync();
        }

        [Authorize]
        [SaveFilter]
        [Route("delete/{id}")]
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await _blogService.DeleteAsync(id);

            return RedirectToAction(nameof(Page), new { page = 1 });
        }

        [Authorize]
        [Route("edit/{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            return await _blogService.GetAsync(id)
                                     .MapAsync(b => new EditPostViewModel(b.Title, b.Content,
                                         new DatesViewModel(b.Dates.Created, b.Dates.Modified)))
                                     .MapAsync(model => (IActionResult) View(nameof(Edit), model))
                                     .WithExceptionAsync(NotFound)
                                     .GetValueOrExceptionAsync();
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
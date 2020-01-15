using System.Linq;
using System.Threading.Tasks;
using AlphaDev.BlogServices.Core;
using AlphaDev.Core;
using AlphaDev.Optional.Extensions;
using AlphaDev.Paging;
using AlphaDev.Paging.Extensions;
using AlphaDev.Services;
using AlphaDev.Web.Core;
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
        private readonly PagesSettings _pagesSettings;

        public PostsController([NotNull] IBlogService blogService, PagesSettings pagesSettings)
        {
            _blogService = blogService;
            _pagesSettings = pagesSettings;
        }

        [Route("page/{page}")]
        public async Task<ActionResult> Page(int page)
        {
            var startPosition = (page - 1) * _pagesSettings.ItemsPerPage + 1;
            return await _blogService.GetOrderedByDatesWithTotalAsync(startPosition, _pagesSettings.ItemsPerPage)
                              .SomeNotEmptyAsync(tuple => tuple.blogs, tuple => NotFound())
                              .MapAsync(tuple => (tuple.total,
                                  blogs: tuple.blogs.Select(blog => new BlogViewModel(blog.Id, blog.Title,
                                      blog.Content,
                                      new DatesViewModel(blog.Dates.Created, blog.Dates.Modified)))))
                              .MapAsync(tuple => tuple.blogs.ToPager(page, tuple.total, _pagesSettings))
                              .MapAsync(x => (ActionResult) View("Index", x))
                              .GetValueOrExceptionAsync();
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
            return await option.MapAsync(model => (ActionResult) View("Post", model)).GetValueOrExceptionAsync();
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
                         .MapAsync(blog => (ActionResult) RedirectToAction(nameof(Index), new { id = blog.Id }))
                         .WithExceptionAsync(() => View(nameof(Create), post))
                         .GetValueOrExceptionAsync();
        }

        [Authorize]
        [SaveFilter]
        [Route("delete/{id}")]
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            return await _blogService.DeleteAsync(id)
                                     .MapAsync(_ => (IActionResult) RedirectToAction(nameof(Page), new { page = 1 }))
                                     .MapExceptionAsync(exception => NotFound())
                                     .GetValueOrExceptionAsync();
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
        public async Task<IActionResult> Edit(int id, EditPostViewModel model)
        {
            return await model.SomeWhen(_ => ModelState.IsValid, () => (IActionResult) View(nameof(Edit), model))
                              .FlatMapAsync(viewModel => _blogService.EditAsync(id, arguments =>
                              {
                                  arguments.Content = viewModel.Content;
                                  arguments.Title = viewModel.Title;
                              }), exception => NotFound())
                              .MapAsync(_ => (IActionResult) RedirectToAction(nameof(Index), new { id }))
                              .GetValueOrExceptionAsync();
        }
    }
}
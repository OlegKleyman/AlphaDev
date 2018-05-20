using System.Linq;
using AlphaDev.Core;
using AlphaDev.Web.Models;
using AlphaDev.Web.Support;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Omego.Extensions.OptionExtensions;
using Optional;

namespace AlphaDev.Web.Controllers
{
    [Route("posts")]
    public class PostsController : Controller
    {
        private readonly IBlogService _blogService;

        public PostsController(IBlogService blogService)
        {
            _blogService = blogService;
        }

        public ViewResult Index()
        {
            var blogs = _blogService.GetAll();
            var model = blogs.Select(blog => new BlogViewModel(blog.Id,
                    blog.Title,
                    blog.Content,
                    new DatesViewModel(blog.Dates.Created, blog.Dates.Modified)))
                .OrderByDescending(viewModel => viewModel.Dates.Created);

            return View(nameof(Index), model);
        }

        [Route("{id}")]
        public ActionResult Index(int id)
        {
            return (TempData?["Model"])
                .SomeNotNull()
                .Map(o => JsonConvert.DeserializeObject<BlogViewModel>(o.ToString(), BlogViewModelConverter.Default))
                .Else(() =>
                    _blogService.Get(id)
                        .Map(foundBlog => new BlogViewModel(foundBlog.Id,
                            foundBlog.Title,
                            foundBlog.Content,
                            new DatesViewModel(foundBlog.Dates.Created, foundBlog.Dates.Modified))))
                .MatchSomeContinue(model => ViewBag.Title = model.Title)
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
        public ActionResult Create(CreatePostViewModel post)
        {
            return ModelState
                .SomeWhen(dictionary => dictionary.IsValid)
                .Map(dictionary => _blogService.Add(new Blog(post.Title, post.Content)))
                .MatchSomeContinue(blog => TempData["Model"] = JsonConvert.SerializeObject(new BlogViewModel(blog.Id,
                    blog.Title, blog.Content,
                    new DatesViewModel(blog.Dates.Created, blog.Dates.Modified)), BlogViewModelConverter.Default))
                .Map(blog => (ActionResult) RedirectToAction(nameof(Index), new {id = blog.Id}))
                .ValueOr(View(nameof(Create), post));
        }

        [Authorize]
        [Route("delete/{id}")]
        [HttpPost]
        public IActionResult Delete(int id)
        {
            _blogService.Delete(id);

            return RedirectToAction(nameof(Index), new {id = (object) null});
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
        public IActionResult Edit(int id, EditPostViewModel model)
        {
            return ModelState
                .SomeWhen(dictionary => dictionary.IsValid)
                .MatchSomeContinue(dictionary => _blogService.Edit(id, arguments =>
                {
                    arguments.Content = model.Content;
                    arguments.Title = model.Title;
                }))
                .Map(dictionary => (IActionResult) RedirectToAction(nameof(Index)))
                .ValueOr(View(nameof(Edit), model));
        }
    }
}
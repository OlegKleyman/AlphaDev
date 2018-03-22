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
            var added = _blogService.Add(new Blog(post.Title, post.Content));
            TempData["Model"] = JsonConvert.SerializeObject(new BlogViewModel(added.Id, added.Title, added.Content,
                new DatesViewModel(added.Dates.Created, added.Dates.Modified)), BlogViewModelConverter.Default);

            return RedirectToAction(nameof(Index), new {id = added.Id});
        }
    }
}
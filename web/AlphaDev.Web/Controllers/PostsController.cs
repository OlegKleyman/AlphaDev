using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AlphaDev.Core;
using AlphaDev.Web.Models;
using Microsoft.AspNetCore.Mvc;

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
                new DatesViewModel(blog.Dates.Created, blog.Dates.Modified))).OrderByDescending(viewModel => viewModel.Dates.Created);

            return View(nameof(Index), model);
        }

        [Route("{id}")]
        public ViewResult Index(int id)
        {
            throw new NotImplementedException();
        }
    }
}
using System.Threading.Tasks;
using AlphaDev.BlogServices.Core;
using AlphaDev.Optional.Extensions;
using AlphaDev.Web.Api.Models;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Optional.Async;
using Optional.Unsafe;
using Blog = AlphaDev.Web.Api.Models.Blog;

namespace AlphaDev.Web.Api.Controllers
{
    [Route("blog")]
    [ApiController]
    public class BlogController : ControllerBase
    {
        private readonly IBlogService _service;

        public BlogController(IBlogService service) => _service = service;

        [Route("GetLatest")]
        [ItemNotNull]
        public async Task<IActionResult> GetLatest()
        {
            return await _service.GetLatestAsync()
                                 .WithExceptionAsync(() => (IActionResult) NotFound())
                                 .MapAsync(b => new Blog
                                 {
                                     Title =b.Title,
                                     Content = b.Content,
                                     Id = b.Id,
                                     Dates = new Models.Dates
                                     {
                                         Created = b.Dates.Created,
                                         Modified = b.Dates.Modified.ValueOrDefault()
                                     }
                                 })
                                 .MapAsync(Ok)
                                 .ExceptionOrValueAsync();
        }
    }
}
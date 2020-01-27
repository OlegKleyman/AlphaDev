using System.Linq;
using System.Threading.Tasks;
using AlphaDev.BlogServices.Core;
using AlphaDev.Core.Extensions;
using AlphaDev.Optional.Extensions;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc;
using Optional.Async;
using Optional.Unsafe;

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
                                 .MapAsync(b => new
                                 {
                                     b.Title,
                                     b.Content,
                                     b.Id,
                                     Dates = new
                                     {
                                         b.Dates.Created,
                                         Modified = b.Dates.Modified.ToNullable()
                                     }
                                 })
                                 .MapAsync(Ok)
                                 .ExceptionOrValueAsync();
        }

        [Route("start/{startPosition}/count/{count}")]
        [NotNull]
        public async Task<IActionResult> GetBlogs(int startPosition, int count)
        {
            return (await _service.GetOrderedByDatesWithTotalAsync(startPosition, count))
                   .To(tuple =>
                   {
                       return new
                       {
                           Total = tuple.total,
                           Values = tuple.blogs.Select(b => new
                                         {
                                             b.Title,
                                             b.Content,
                                             b.Id,
                                             Dates = new
                                             {
                                                 b.Dates.Created,
                                                 Modified = b.Dates.Modified.ValueOrDefault()
                                             }
                                         })
                                         .ToArray()
                       };
                   })
                   .To(Ok);
        }

        [Route("{id}")]
        [ItemNotNull]
        public async Task<IActionResult> Get(int id)
        {
            return await _service.GetAsync(id)
                                 .WithExceptionAsync(NotFound)
                                 .MapAsync(b => new
                                 {
                                     b.Content,
                                     b.Title,
                                     b.Id,
                                     Dates = new
                                     {
                                         b.Dates.Created,
                                         Modified = b.Dates.Modified.ToNullable()
                                     }
                                 })
                                 .MapAsync(b => (IActionResult) Ok(b))
                                 .ValueOrExceptionAsync();
        }
    }
}
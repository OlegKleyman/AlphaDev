using System.Net;
using System.Threading.Tasks;
using AlphaDev.BlogServices.Core;
using AlphaDev.Optional.Extensions;
using Optional;
using Optional.Async;
using Refit;
using BlogModel = AlphaDev.Services.Web.Models.Blog;

namespace AlphaDev.BlogServices.Web.Extentions
{
    public static class BlogModelTaskExtensions
    {
        public static Task<Option<BlogBase>> ToOptionAsync(this Task<ApiResponse<BlogModel>> task)
        {
            return task.SomeWhenAsync(response => response.IsSuccessStatusCode,
                           response => response.Error)
                       .MapExceptionAsync(response => response.SomeWhen(exception =>
                           exception.StatusCode != HttpStatusCode.NotFound))
                       .MapExceptionAsync(option => option.WithException(Option.None<BlogBase>))
                       .MapAsync(response => (BlogBase) new Blog(response.Content.Id,
                           response.Content.Title, response.Content.Content,
                           new Dates(response.Content.Dates.Created,
                               response.Content.Dates.Modified.ToOption())))
                       .MatchAsync(b => b.Some(),
                           option => option.Match(exception => throw exception, option1 => option1));
        }
    }
}

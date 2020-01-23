using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using AlphaDev.BlogServices.Core;
using AlphaDev.BlogServices.Web.Extentions;
using AlphaDev.Core;
using AlphaDev.Core.Extensions;
using AlphaDev.Optional.Extensions;
using AlphaDev.Services.Web;
using JetBrains.Annotations;
using Optional;
using Optional.Async;
using Refit;
using BlogModel = AlphaDev.Services.Web.Models.Blog;
using DatesModel = AlphaDev.Services.Web.Models.Dates;

namespace AlphaDev.BlogServices.Web
{
    public class BlogService : IBlogService
    {
        private readonly IBlogRestService _blogRestService;
        private readonly IDateProvider _dateProvider;

        public BlogService(IBlogRestService blogRestService, IDateProvider dateProvider)
        {
            _blogRestService = blogRestService;
            _dateProvider = dateProvider;
        }

        public async Task<Option<BlogBase>> GetLatestAsync() => await _blogRestService.GetLatest().ToOptionAsync();

        public async Task<Option<BlogBase>> GetAsync(int id) => await _blogRestService.Get(id).ToOptionAsync();

        public async Task<BlogBase> AddAsync([NotNull] BlogBase blog)
        {
            return await _blogRestService.Add(new BlogModel(new DatesModel
                                         {
                                             Created = _dateProvider.UtcNow
                                         })
                                         {
                                             Content = blog.Content,
                                             Title = blog.Title
                                         })
                                         .SomeWhenAsync(response => response.IsSuccessStatusCode,
                                             response => response.Error)
                                         .MapAsync(response => (BlogBase) new Blog(response.Content.Id,
                                             response.Content.Title, response.Content.Content,
                                             new Dates(response.Content.Dates.Created,
                                                 response.Content.Dates.Modified.ToOption())))
                                         .ValueOrAsync(exception => throw exception);
        }

        public async Task<Option<Unit, ObjectNotFoundException<BlogBase>>> DeleteAsync(int id)
        {
            return await _blogRestService.Delete(1)
                                         .SomeWhenExceptionAsync(
                                             message => message.IsSuccessStatusCode ||
                                                        message.StatusCode == HttpStatusCode.NotFound, message =>
                                                 ApiException.Create(message.RequestMessage,
                                                     message.RequestMessage.Method, message))
                                         .ValueOrAsync(exception => throw exception)
                                         .ToAsync(message =>
                                             message.SomeWhen(responseMessage => responseMessage.IsSuccessStatusCode))
                                         .MapAsync(message => Unit.Value)
                                         .WithExceptionAsync(() =>
                                             new ObjectNotFoundException<BlogBase>((nameof(BlogBase.Id), id)));
        }

        public async Task<Option<Unit, ObjectNotFoundException<BlogBase>>> EditAsync(int id, Action<BlogEditArguments> edit)
        {
            var arguments = new BlogEditArguments();
            edit(arguments);

            var blogToEdit = new BlogModel(new DatesModel
            {
                Modified = _dateProvider.UtcNow
            })
            {
                Content = arguments.Content,
                Title = arguments.Title
            };

            return await _blogRestService.Edit(id, blogToEdit)
                                         .SomeWhenExceptionAsync(
                                             message => message.IsSuccessStatusCode ||
                                                        message.StatusCode == HttpStatusCode.NotFound,
                                             message => ApiException.Create(message.RequestMessage,
                                                 message.RequestMessage.Method, message))
                                         .ValueOrAsync(exception => throw exception)
                                         .ToAsync(message =>
                                             message.SomeWhen(responseMessage => responseMessage.IsSuccessStatusCode))
                                         .MapAsync(message => Unit.Value)
                                         .WithExceptionAsync(() =>
                                             new ObjectNotFoundException<BlogBase>((nameof(BlogBase.Id), id)));
        }

        public async Task<IEnumerable<BlogBase>> GetOrderedByDatesAsync(int start, int count)
        {
            return await _blogRestService.Get(start, count)
                                         .ToAsync(segmented => segmented.Values.Select(blog => new Blog(blog.Id, blog.Title,
                                             blog.Content,
                                             new Dates(blog.Dates.Created, blog.Dates.Modified.ToOption()))));
        }

        public async Task<(int total, IEnumerable<BlogBase> blogs)> GetOrderedByDatesWithTotalAsync(int start, int count)
        {
            return await _blogRestService.Get(start, count)
                                         .ToAsync(segmented => (segmented.Total,
                                             segmented.Values.Select(blog => new Blog(blog.Id, blog.Title,
                                                 blog.Content,
                                                 new Dates(blog.Dates.Created,
                                                     blog.Dates.Modified.ToOption())))));
        }

        public async Task<int> GetCountAsync() =>
            await _blogRestService.Get(1, 1).ToAsync(segmented => segmented.Total);
    }
}

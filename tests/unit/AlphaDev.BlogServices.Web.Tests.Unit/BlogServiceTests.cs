using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using AlphaDev.BlogServices.Core;
using AlphaDev.BlogServices.Web.Extentions;
using AlphaDev.Core;
using AlphaDev.Services.Web;
using FastDeepCloner;
using FluentAssertions;
using FluentAssertions.Optional.Extensions;
using JetBrains.Annotations;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Optional;
using Refit;
using Xunit;
using Blog = AlphaDev.Services.Web.Models.Blog;
using Dates = AlphaDev.Services.Web.Models.Dates;

namespace AlphaDev.BlogServices.Web.Tests.Unit
{
    public class BlogServiceTests
    {
        [Fact]
        public async Task GetLatestAsyncReturnsSomeBlogFromRestService()
        {
            var blogRestService = Substitute.For<IBlogRestService>();
            var blog = new Blog(new Dates
            {
                Created = new DateTime(2010, 1, 1),
                Modified = new DateTime(2012, 12, 28)
            })
            {
                Title = nameof(Blog.Title),
                Content = nameof(Blog.Content),
                Id = 1
            };

            blogRestService.GetLatest().Returns(new ApiResponse<Blog>(new HttpResponseMessage(), blog));
            var service = GetBlogService(blogRestService, Substitute.For<IDateProvider>());
            var result = await service.GetLatestAsync();
            result.Should().HasValueEquivalentTo(new
            {
                blog.Title,
                blog.Content,
                blog.Id,
                Dates = new
                {
                    blog.Dates.Created,
                    Modified = blog.Dates.Modified.ToOption()
                }
            });
        }

        [Fact]
        public async Task GetAsyncReturnsSomeBlogFromRestService()
        {
            var blogRestService = Substitute.For<IBlogRestService>();
            var blog = new Blog(new Dates
            {
                Created = new DateTime(2010, 1, 1),
                Modified = new DateTime(2012, 12, 28)
            })
            {
                Title = nameof(Blog.Title),
                Content = nameof(Blog.Content),
                Id = 1
            };

            blogRestService.Get(1).Returns(new ApiResponse<Blog>(new HttpResponseMessage(), blog));
            var service = GetBlogService(blogRestService, Substitute.For<IDateProvider>());
            var result = await service.GetAsync(1);
            result.Should().HasValueEquivalentTo(new
            {
                blog.Title,
                blog.Content,
                blog.Id,
                Dates = new
                {
                    blog.Dates.Created,
                    Modified = blog.Dates.Modified.ToOption()
                }
            });
        }

        [Fact]
        public async Task AddAsyncReturnsBlogFromRestServiceWhenAddedSuccessfully()
        {
            var dateProvider = Substitute.For<IDateProvider>();
            dateProvider.UtcNow.Returns(new DateTime(2010, 1, 1));
            var blogRestService = Substitute.For<IBlogRestService>();
            var blog = new Blog(new Dates
            {
                Created = dateProvider.UtcNow
            })
            {
                Id = 1,
                Title = nameof(Blog.Title),
                Content = nameof(Blog.Content)
            };

            blogRestService
                .Add(Arg.Is<Blog>(b =>
                    b.Dates.Created == dateProvider.UtcNow && b.Title == nameof(BlogBase.Title) &&
                    b.Content == nameof(BlogBase.Content)))
                .Returns(new ApiResponse<Blog>(new HttpResponseMessage(), blog));
         
            var service = GetBlogService(blogRestService, dateProvider);
            var result = await service.AddAsync(new Core.Blog(nameof(BlogBase.Title), nameof(BlogBase.Content)));
            result.Should().BeEquivalentTo(new
            {
                blog.Title,
                blog.Content,
                blog.Id,
                Dates = new
                {
                    blog.Dates.Created,
                    Modified = blog.Dates.Modified.ToOption()
                }
            });
        }

        [Fact]
        public async Task AddAsyncThrowsExceptionWhenResponseIsNotSuccessful()
        {
            var dateProvider = Substitute.For<IDateProvider>();
            dateProvider.UtcNow.Returns(new DateTime(2010, 1, 1));
            var blogRestService = Substitute.For<IBlogRestService>();

            var responseMessage = new HttpResponseMessage(HttpStatusCode.BadRequest);
            var error = await ApiException.Create(default, default, responseMessage);
            var response = new ApiResponse<Blog>(responseMessage, new Blog(new Dates()), error);
            blogRestService.Add(Arg.Any<Blog>()).Returns(response);

            var service = GetBlogService(blogRestService, dateProvider);
            Func<Task> addAsync = () => service.AddAsync(new Core.Blog(string.Empty, string.Empty));
            addAsync.Should().ThrowExactly<ApiException>();
        }

        [Fact]
        public async Task EditAsyncReturnsObjectNotFoundExceptionWhenApiExceptionIsThrownAndStatusCodeIsNotFound()
        {
            var blogRestService = Substitute.For<IBlogRestService>();

            var responseMessage = new HttpResponseMessage(HttpStatusCode.NotFound);
            blogRestService.Edit(1, Arg.Any<Blog>()).Returns(responseMessage);

            var service = GetBlogService(blogRestService, Substitute.For<IDateProvider>());
            var result = await service.EditAsync(1, arguments =>
            {
                arguments.Content = string.Empty;
                arguments.Title = string.Empty;
            });

            result.Should()
                  .HasExceptionEquivalentTo(new
                  {
                      Type = typeof(BlogBase),
                      Criteria = new Dictionary<string, object>
                      {
                          [nameof(BlogBase.Id)] = new[]{1}
                      },
                      Message = "AlphaDev.BlogServices.Core.BlogBase was not found based on the criteria." +
                                Environment.NewLine + "Id:1"
                  });
        }

        [Fact]
        public void EditAsyncThrowsApiExceptionWhenWhenRestServiceThrowsApiExceptionWithStatusCodeNotNotFound()
        {
            var blogRestService = Substitute.For<IBlogRestService>();

            var responseMessage = new HttpResponseMessage(HttpStatusCode.BadGateway)
            {
                RequestMessage = new HttpRequestMessage()
            };

            blogRestService.Edit(1, Arg.Any<Blog>()).Returns(responseMessage);

            var service = GetBlogService(blogRestService, Substitute.For<IDateProvider>());
            Func<Task> editAsync =()=> service.EditAsync(1, arguments =>
            {
                arguments.Content = string.Empty;
                arguments.Title = string.Empty;
            });

            editAsync.Should().ThrowExactly<ApiException>();
        }

        [Fact]
        public async Task EditAsyncReturnsSomeUnitWhenEditWasSuccessful()
        {
            var blogRestService = Substitute.For<IBlogRestService>();
            var responseMessage = new HttpResponseMessage(HttpStatusCode.OK);
            blogRestService.Edit(1, Arg.Any<Blog>()).Returns(responseMessage);
            var service = GetBlogService(blogRestService, Substitute.For<IDateProvider>());
            var result = await service.EditAsync(1, arguments =>
            {
                arguments.Content = string.Empty;
                arguments.Title = string.Empty;
            });

            result.Should().HaveSome();
        }

        [Fact]
        public async Task DeleteAsyncReturnsObjectNotFoundExceptionWhenApiExceptionIsThrownAndStatusCodeIsNotFound()
        {
            var blogRestService = Substitute.For<IBlogRestService>();

            var responseMessage = new HttpResponseMessage(HttpStatusCode.NotFound);
            blogRestService.Delete(1).Returns(responseMessage);

            var service = GetBlogService(blogRestService, Substitute.For<IDateProvider>());
            var result = await service.DeleteAsync(1);

            result.Should()
                  .HasExceptionEquivalentTo(new
                  {
                      Type = typeof(BlogBase),
                      Criteria = new Dictionary<string, object>
                      {
                          [nameof(BlogBase.Id)] = new[] { 1 }
                      },
                      Message = "AlphaDev.BlogServices.Core.BlogBase was not found based on the criteria." +
                                Environment.NewLine + "Id:1"
                  });
        }

        [Fact]
        public void DeleteAsyncThrowsApiExceptionWhenWhenRestServiceThrowsApiExceptionWithStatusCodeNotNotFound()
        {
            var blogRestService = Substitute.For<IBlogRestService>();

            var responseMessage = new HttpResponseMessage(HttpStatusCode.BadGateway)
            {
                RequestMessage = new HttpRequestMessage()
            };
            blogRestService.Delete(1).Returns(responseMessage);

            var service = GetBlogService(blogRestService, Substitute.For<IDateProvider>());
            Func<Task> deleteAsync = () => service.DeleteAsync(1);
            deleteAsync.Should().ThrowExactly<ApiException>();
        }

        [Fact]
        public async Task DeleteAsyncReturnsSomeUnitWhenDeleteWasSuccessful()
        {
            var blogRestService = Substitute.For<IBlogRestService>();
            blogRestService.Delete(1).Returns(new HttpResponseMessage(HttpStatusCode.OK));
            var service = GetBlogService(blogRestService, Substitute.For<IDateProvider>());
            var result = await service.DeleteAsync(1);
            result.Should().HaveSome();
        }

        [NotNull]
        private BlogService GetBlogService(IBlogRestService blogRestService, IDateProvider dateProvider) =>
            new BlogService(blogRestService, dateProvider);
    }
}

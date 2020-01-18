using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using AlphaDev.BlogServices.Core;
using AlphaDev.BlogServices.Web.Extentions;
using FluentAssertions;
using FluentAssertions.Optional;
using FluentAssertions.Optional.Extensions;
using Optional;
using Optional.Unsafe;
using Refit;
using Xunit;
using Blog = AlphaDev.Services.Web.Models.Blog;
using Dates = AlphaDev.Services.Web.Models.Dates;

namespace AlphaDev.BlogServices.Web.Tests.Unit.Extensions
{
    public class BlogModelTaskExtensionsTests
    {
        [Fact]
        public async Task ToOptionAsyncThrowsExceptionWhenResponseIsNotSuccessfulAndNot404()
        {
            var responseMessage = new HttpResponseMessage(HttpStatusCode.BadRequest);
            var error = await ApiException.Create(default, default, responseMessage);
            var response = new ApiResponse<Blog>(responseMessage, new Blog(), error);
            Func<Task> toOptionAsync = () => Task.FromResult(response).ToOptionAsync();
            toOptionAsync.Should().ThrowExactly<ApiException>();
        }

        [Fact]
        public async Task ToOptionAsyncReturnsNoneWhen404IsReturned()
        {
            var responseMessage = new HttpResponseMessage(HttpStatusCode.NotFound);
            var error = await ApiException.Create(default, default, responseMessage);
            var response = new ApiResponse<Blog>(responseMessage, new Blog(), error);
            var result = await Task.FromResult(response).ToOptionAsync();
            result.Should().BeNone();
        }

        [Fact]
        public async Task ToOptionAsyncReturnsBlogWhenResponseIsSuccessful()
        {
            var responseMessage = new HttpResponseMessage(HttpStatusCode.OK);
            var blog = new Blog
            {
                Title = nameof(Blog.Title),
                Content = nameof(Blog.Content),
                Id = 1,
                Dates = new Dates
                {
                    Created = new DateTime(2010,1,1),
                    Modified = new DateTime(2012,12,28)
                }
            };

            var response = new ApiResponse<Blog>(responseMessage, blog);
            var result = await Task.FromResult(response).ToOptionAsync();
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
    }
}

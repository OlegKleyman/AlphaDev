using System;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using AlphaDev.Core.Data.Contexts;
using AlphaDev.Optional.Extensions;
using AlphaDev.Services.Web;
using AlphaDev.Services.Web.Models;
using FluentAssertions;
using FluentAssertions.Optional.Extensions;
using Optional;
using Optional.Async;
using Refit;
using TechTalk.SpecFlow;
using Blog = AlphaDev.Core.Data.Entities.Blog;

namespace AlphaDev.Web.Api.Tests.Integration.FeatureSteps
{
    [Binding]
    public class GetBlog : Steps
    {
        [Given(@"There are blogs")]
        public async Task GivenThereAreBlogs()
        {
            await GivenIHaveBlogs(100);
        }

        [Given(@"There are no blogs")]
        public void GivenThereAreNoBlogs()
        {
        }

        [Given(@"I am an API consumer")]
        public void GivenIAmAnApiConsumer()
        {
        }

        [When(@"I make a request to get the latest blog")]
        public async Task WhenIMakeARequestToGetTheLatestBlog()
        {
            var service = RestService.For<IBlogRestService>(ScenarioContext.Get<Uri>("BLOG_SERVICE_URL").AbsoluteUri);
            ScenarioContext.Set(await service.GetLatest()
                                              .SomeWhenAsync(response => response.IsSuccessStatusCode,
                                                  response => response.Error)
                                              .MapAsync(response => response.Content));
        }

        [Then(@"I will receive a (\d+) response")]
        public void ThenIWillReceiveAResponse(int statusCode)
        {
            var option = ScenarioContext.Get<Option<Services.Web.Models.Blog, ApiException>>();
            option.Should().BeNone().Which.StatusCode.Should().BeEquivalentTo(statusCode);
        }

        [Then(@"the latest blog is returned")]
        public void ThenTheLatestBlogIsReturned()
        {
            var latestBlog = ScenarioContext.Get<ImmutableArray<Blog>>()
                                            .OrderByDescending(blog => blog.Created)
                                            .First();

            var option = ScenarioContext.Get<Option<Services.Web.Models.Blog, ApiException>>();
            option.Should().HasValueEquivalentTo(new
            {
                latestBlog.Title,
                latestBlog.Content ,
                Dates = new
                {
                    latestBlog.Created,
                    latestBlog.Modified
                }
            });
        }

        [Given(@"I have (\d+) blogs")]
        public async Task GivenIHaveBlogs(int blogCount)
        {
            var blogs = Enumerable.Range(1, blogCount)
                                  .Select(i => new Blog
                                  {
                                      Modified = DateTime.MinValue.AddDays(i + 100),
                                      Title = nameof(Blog.Title) + i,
                                      Created = DateTime.MinValue.AddDays(i),
                                      Content = nameof(Blog.Content) + i
                                  }).ToImmutableArray();

            var context = ScenarioContext.Get<BlogContext>();
            await context.AddRangeAsync(blogs);
            await context.SaveChangesAsync();

            ScenarioContext.Set(blogs);
        }

        [When(@"I make a request to get (\d+) blogs from position (\d+)")]
        public async Task WhenIMakeARequestToGetBlogsFromPosition(int blogGetCount, int startPosition)
        {
            var service = RestService.For<IBlogRestService>(ScenarioContext.Get<Uri>("BLOG_SERVICE_URL").AbsoluteUri);
            var blogs = await service.Get(startPosition, blogGetCount);
            ScenarioContext.Set(blogs);
            ScenarioContext["TOTAL_GET_BLOGS"] = blogGetCount;
            ScenarioContext["START_GET_BLOGS_POSITION"] = startPosition;
        }

        [Then(@"those blogs are returned")]
        public void ThenThoseBlogsAreReturned()
        {
            var returnedBlogs = ScenarioContext.Get<Segmented<Services.Web.Models.Blog>>();
            var actualBlogs = ScenarioContext.Get<ImmutableArray<Blog>>();
            returnedBlogs.Total.Should().Be(actualBlogs.Length);
            var expectedBlogs = actualBlogs
                              .OrderByDescending(blog => blog.Modified)
                              .ThenByDescending(blog => blog.Created)
                              .Skip(ScenarioContext.Get<int>("START_GET_BLOGS_POSITION") - 1)
                              .Take(ScenarioContext.Get<int>("TOTAL_GET_BLOGS"))
                              .Select(blog => new
                              {
                                  blog.Title,
                                  blog.Content,
                                  Dates = new
                                  {
                                      blog.Created,
                                      blog.Modified
                                  }
                              });

            returnedBlogs.Values.Should().BeEquivalentTo(expectedBlogs);
        }
    }
}

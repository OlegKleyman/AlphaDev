using System;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using AlphaDev.Core.Data.Contexts;
using AlphaDev.Optional.Extensions;
using AlphaDev.Services.Web;
using AlphaDev.Services.Web.Models;
using FluentAssertions;
using Refit;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;
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
            await service.GetLatest()
                         .SomeWhenAsync(apiResponse => apiResponse.IsSuccessStatusCode,
                             apiResponse => apiResponse.Error)
                         .MatchAsync(apiResponse => ScenarioContext.Set(apiResponse.Content),
                             exception => ScenarioContext.Set(exception));
        }

        [Then(@"I will receive a (\d+) response")]
        public void ThenIWillReceiveAResponse(int statusCode)
        {
            ScenarioContext.Get<ApiException>()?.StatusCode.Should().BeEquivalentTo(statusCode);
        }

        [Then(@"the latest blog is returned")]
        public void ThenTheLatestBlogIsReturned()
        {
            var latestBlog = ScenarioContext.Get<ImmutableArray<Blog>>()
                                            .OrderByDescending(blog => blog.Created)
                                            .First();

            var blog = ScenarioContext.Get<Services.Web.Models.Blog>();
            blog.Should().BeEquivalentTo(new
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

        [Given(@"There is a blog")]
        public async Task GivenThereIsABlog(Table blogTable)
        {
            var context = ScenarioContext.Get<BlogContext>();
            var blog = blogTable.CreateSet<Blog>().Single();
            await context.Blogs.AddAsync(blog);
            await context.SaveChangesAsync();
            ScenarioContext.Set(context.Blogs.ToImmutableArray());
            ScenarioContext.Set(blog);
        }

        [When(@"I make a request to get that blog")]
        public async Task WhenIMakeARequestToGetThatBlog()
        {
            await WhenIMakeARequestToGetABlogByTheIdOf(ScenarioContext.Get<Blog>().Id);
        }

        [Then(@"the blog is returned")]
        public void ThenTheBlogIsReturned()
        {
            var blog = ScenarioContext.Get<Blog>();
            ScenarioContext.Get<Services.Web.Models.Blog>().Should().BeEquivalentTo(new
            {
                blog.Id,
                blog.Title,
                blog.Content,
                Dates = new
                {
                    blog.Created,
                    blog.Modified
                }
            });
        }

        [When(@"I make a request to get a blog by the id of (\d+)")]
        public async Task WhenIMakeARequestToGetABlogByTheIdOf(int id)
        {
            var service = RestService.For<IBlogRestService>(ScenarioContext.Get<Uri>("BLOG_SERVICE_URL").AbsoluteUri);
            var response = await service.Get(id);
            await service.Get(id)
                         .SomeWhenAsync(apiResponse => apiResponse.IsSuccessStatusCode,
                             apiResponse => apiResponse.Error)
                         .MatchAsync(apiResponse => ScenarioContext.Set(response.Content),
                             exception => ScenarioContext.Set(exception));
        }
    }
}

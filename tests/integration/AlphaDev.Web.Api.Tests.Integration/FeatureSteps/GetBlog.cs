using System;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using AlphaDev.Core.Data.Contexts;
using AlphaDev.Core.Data.Entities;
using AlphaDev.Optional.Extensions;
using AlphaDev.Services.Web;
using FluentAssertions.Optional.Extensions;
using Optional;
using Optional.Async;
using Refit;
using TechTalk.SpecFlow;

namespace AlphaDev.Web.Api.Tests.Integration.FeatureSteps
{
    [Binding]
    public class GetBlog : Steps
    {
        [Given(@"There are blogs")]
        public async Task GivenThereAreBlogs()
        {
            var blogs = Enumerable.Range(1, 100)
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
    }
}

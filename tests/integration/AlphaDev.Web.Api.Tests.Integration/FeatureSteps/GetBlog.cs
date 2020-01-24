using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using AlphaDev.Core.Data.Contexts;
using AlphaDev.Core.Data.Entities;
using AlphaDev.Optional.Extensions;
using AlphaDev.Services.Web;
using AspNetCoreTestServer.Core;
using FluentAssertions;
using FluentAssertions.Optional.Extensions;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Optional;
using Optional.Async;
using Refit;
using TechTalk.SpecFlow;

namespace AlphaDev.Web.Api.Tests.Integration.FeatureSteps
{
    [Binding]
    public class GetBlog : Steps
    {
        private readonly ScenarioContext _scenarioContext;
        private readonly BlogContext _blogContext;
        private readonly IWebServer _server;

        public GetBlog(ScenarioContext scenarioContext, BlogContext blogContext, IWebServer server)
        {
            _scenarioContext = scenarioContext;
            _blogContext = blogContext;
            _server = server;
        }

        [BeforeScenario("blog")]
        public void InitializeDatabase()
        {
            _blogContext.Database.Migrate();
        }

        [BeforeScenario("blog")]
        public async Task StartServer()
        {
            var state = await _server.StartAsync<Startup>(typeof(Startup).Assembly, Option.None<string>(),
                new Dictionary<string, string>
                {
                    ["connectionStrings:default"] = _blogContext.Database.GetDbConnection().ConnectionString
                });
            _scenarioContext["BLOG_SERVICE_URL"] = state.Endpoint;
        }

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

            await _blogContext.AddRangeAsync(blogs);
            await _blogContext.SaveChangesAsync();

            _scenarioContext.Set(blogs);
        }

        [Given(@"I am an API consumer")]
        public void GivenIAmAnApiConsumer()
        {
        }

        [When(@"I make a request to get the latest blog")]
        public async Task WhenIMakeARequestToGetTheLatestBlog()
        {
            var service = RestService.For<IBlogRestService>(_scenarioContext.Get<Uri>("BLOG_SERVICE_URL").AbsoluteUri);
            _scenarioContext.Set(await service.GetLatest()
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

            var option = _scenarioContext.Get<Option<Services.Web.Models.Blog, ApiException>>();
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

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AlphaDev.Core;
using AlphaDev.Core.Extensions;
using AlphaDev.Web.Controllers;
using AlphaDev.Web.Core.Extensions;
using AlphaDev.Web.Core.Support;
using AlphaDev.Web.Models;
using FluentAssertions;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using NSubstitute;
using Optional;
using Xunit;

namespace AlphaDev.Web.Tests.Unit.Controllers
{
    public class PostsControllerTests
    {
        [NotNull]
        private PostsController GetPostsController()
        {
            var blogService = Substitute.For<IBlogService>();
            blogService.GetLatestAsync().Returns(((BlogBase) new Blog(default, null, null, default)).Some());
            blogService.GetCountAsync(Arg.Any<int>()).Returns(1);
            return GetPostsController(blogService);
        }

        [NotNull]
        private PostsController GetPostsController([NotNull] IBlogService blogService) =>
            new PostsController(blogService);

        [Fact]
        public void CreateShouldReturnCreateView()
        {
            var controller = GetPostsController();

            controller.Create().ViewName.Should().BeEquivalentTo("Create");
        }

        [Fact]
        public async Task CreateShouldReturnCreateViewWhenModelStateIsInvalid()
        {
            var controller = GetPostsController(Substitute.For<IBlogService>());

            controller.ModelState.AddModelError("test", "test");

            (await controller.Create(default))
                      .Should()
                      .BeOfType<ViewResult>()
                      .Which.ViewName.Should()
                      .BeEquivalentTo("Create");
        }

        [Fact]
        public async Task CreateShouldReturnViewWithSameModelWhenModelStateIsInvalid()
        {
            var controller = GetPostsController(Substitute.For<IBlogService>());

            controller.ModelState.AddModelError("test", "test");

            var post = new CreatePostViewModel("title", "content");

            (await controller.Create(post)).Should().BeOfType<ViewResult>().Which.Model.Should().BeEquivalentTo(post);
        }

        [Fact]
        public async Task CreateShouldRouteIdArgument()
        {
            var controller = GetPostsController(Substitute.For<IBlogService>());

            controller.TempData = new TempDataDictionary(new DefaultHttpContext(), Substitute.For<ITempDataProvider>());

            var post = new CreatePostViewModel("title", "content");

            (await controller.Create(post))
                             .Should()
                             .BeOfType<RedirectToActionResult>()
                             .Which.RouteValues.Should()
                             .ContainKey("id")
                             .WhichValue.Should()
                             .BeEquivalentTo(default(int));
        }

        [Fact]
        public async Task CreateShouldRouteToIndexAction()
        {
            var controller = GetPostsController(Substitute.For<IBlogService>());

            controller.TempData = new TempDataDictionary(new DefaultHttpContext(), Substitute.For<ITempDataProvider>());

            var post = new CreatePostViewModel("title", "content");

            (await controller.Create(post))
                             .Should()
                             .BeOfType<RedirectToActionResult>()
                             .Which.ActionName.Should()
                             .BeEquivalentTo("Index");
        }

        [Fact]
        public async Task DeleteShouldDeleteBlogWithMatchingId()
        {
            var service = Substitute.For<IBlogService>();
            var controller = GetPostsController(service);

            const int id = 10;
            await controller.Delete(id);

            await service.Received().DeleteAsync(id);
        }

        [Fact]
        public async Task DeleteShouldRedirectToDefaultPageAction()
        {
            var controller = GetPostsController(Substitute.For<IBlogService>());

            var result = (await controller.Delete(default)).Should().BeOfType<RedirectToActionResult>();
            result.Which.ActionName.Should().BeEquivalentTo("Page");
            result.Which.RouteValues["id"].Should().BeNull();
        }

        [Fact]
        public async Task EditShouldEditPostWithModelData()
        {
            var blogService = Substitute.For<IBlogService>();
            var editArguments = new BlogEditArguments();

            await blogService.EditAsync(1, Arg.Invoke(editArguments));
            var controller = GetPostsController(blogService);

            var post = new EditPostViewModel("title", "content", default);
            await controller.Edit(1, post);

            editArguments.Should().BeEquivalentTo(new { post.Content, post.Title });
        }

        [Fact]
        public async Task EditShouldReturnEditViewWhenBlogIsFound()
        {
            var blogService = Substitute.For<IBlogService>();
            blogService.GetAsync(Arg.Any<int>()).Returns(Option.Some(Substitute.For<BlogBase>()));

            var controller = GetPostsController(blogService);

            (await controller.Edit(default))
                      .Should()
                      .BeOfType<ViewResult>()
                      .Which.ViewName.Should()
                      .BeEquivalentTo("Edit");
        }

        [Fact]
        public async Task EditShouldReturnEditViewWhenModelStateIsInvalid()
        {
            var controller = GetPostsController(Substitute.For<IBlogService>());

            controller.ModelState.AddModelError("test", "test");

            var post = new EditPostViewModel("title", "content", new DatesViewModel());

            (await controller.Edit(default, post))
                      .Should()
                      .BeOfType<ViewResult>()
                      .Which.ViewName.Should()
                      .BeEquivalentTo("Edit");
        }

        [Fact]
        public async Task EditShouldReturnEditViewWithModelWhenBlogIsFound()
        {
            var blog = Substitute.For<BlogBase>();
            blog.Id.Returns(default(int));
            blog.Content.Returns("content");
            blog.Title.Returns("title");
            blog.Dates.Returns(default(Dates));

            var blogService = Substitute.For<IBlogService>();
            blogService.GetAsync(Arg.Any<int>()).Returns(Option.Some(blog));

            var controller = GetPostsController(blogService);

            (await controller.Edit(default))
                      .Should()
                      .BeOfType<ViewResult>()
                      .Which.Model.Should()
                      .BeEquivalentTo(
                          new
                          {
                              Title = "title",
                              Content = "content",
                              Dates = new DatesViewModel(blog.Dates.Created, blog.Dates.Modified)
                          });
        }

        [Fact]
        public async Task EditShouldReturnViewWithSameModelWhenModelStateIsInvalid()
        {
            var controller = GetPostsController(Substitute.For<IBlogService>());

            controller.ModelState.AddModelError("test", "test");

            var post = new EditPostViewModel("title", "content", new DatesViewModel());

            (await controller.Edit(default, post)).Should().BeOfType<ViewResult>().Which.Model.Should().BeEquivalentTo(post);
        }

        [Fact]
        public async Task EditShouldRouteToIndexActionWhenModelStateIsValid()
        {
            var controller = GetPostsController(Substitute.For<IBlogService>());

            (await controller.Edit(default, new EditPostViewModel(string.Empty, string.Empty, default)))
                      .Should()
                      .BeOfType<RedirectToActionResult>()
                      .Which.ActionName.Should()
                      .BeEquivalentTo("Index");
        }

        [Fact]
        public async Task IndexShouldReturn404StatusIfNoBlogFound()
        {
            var blogService = Substitute.For<IBlogService>();
            var controller = GetPostsController(blogService);

            blogService.GetAsync(Arg.Any<int>()).Returns(Option.None<BlogBase>());

            (await controller.Index(default)).Should().BeOfType<NotFoundResult>().Which.StatusCode.Should().Be(404);
        }

        [Fact]
        public async Task IndexShouldReturnBlogModelWithValuesSetFromTheBlogServiceWhenTempDataOrModelDontExist()
        {
            const int id = 123;
            BlogBase blog = new Blog(id,
                "title",
                "content",
                new Dates(new DateTime(2015, 7, 27), Option.Some(new DateTime(2016, 8, 28))));

            var blogService = Substitute.For<IBlogService>();
            blogService.GetAsync(id).Returns(blog.Some());

            var controller = GetPostsController(blogService);

            (await controller.Index(id))
                      .Should()
                      .BeOfType<ViewResult>()
                      .Which.Model.Should()
                      .BeEquivalentTo(
                          new
                          {
                              blog.Id, blog.Title, blog.Content,
                              Dates = new { blog.Dates.Created, blog.Dates.Modified }
                          });
        }

        [Fact]
        public async Task IndexShouldReturnPostViewWhenPostIsFound()
        {
            BlogBase blog = new Blog(123,
                "title",
                "content",
                new Dates(new DateTime(2015, 7, 27), Option.Some(new DateTime(2016, 8, 28))));

            var blogService = Substitute.For<IBlogService>();
            blogService.GetAsync(Arg.Any<int>()).Returns(Option.Some(blog));

            var controller = GetPostsController(blogService);

            (await controller.Index(default)).Should().BeOfType<ViewResult>();
        }

        [Fact]
        public async Task IndexShouldReturnPostViewWithBlogTitleAsPageTitle()
        {
            const int id = 123;
            BlogBase blog = new Blog(id,
                "title",
                "content",
                new Dates(new DateTime(2015, 7, 27), Option.Some(new DateTime(2016, 8, 28))));

            var blogService = Substitute.For<IBlogService>();
            blogService.GetAsync(id).Returns(Option.Some(blog));

            var controller = GetPostsController(blogService);

            (await controller.Index(id))
                      .Should()
                      .BeOfType<ViewResult>()
                      .Which.ViewData["Title"]
                      .Should()
                      .BeEquivalentTo("title");
        }

        [Fact]
        public async Task PageShouldGetLessThanElevenItems()
        {
            var blogService = Substitute.For<IBlogService>();
            blogService.GetCountAsync(Arg.Any<int>()).Returns(1);
            const int page = 9;
            await GetPostsController(blogService).Page(page);
            var value = page.ToPositiveInteger().ToStartPosition(10.ToPositiveInteger()).Value;
            await blogService.Received(1).GetOrderedByDatesAsync(Arg.Is(value), Arg.Is(10));
        }

        [Fact]
        public async Task PageShouldReturnBlogModelsAssignableToEnumerableOfBlogViewModel()
        {
            var blogService = Substitute.For<IBlogService>();
            blogService.GetOrderedByDatesAsync(Arg.Any<int>(), Arg.Any<int>())
                       .Returns(new[] { new Blog(string.Empty, string.Empty) });
            blogService.GetCountAsync(Arg.Any<int>()).Returns(1);
            var controller = GetPostsController(blogService);
            (await controller.Page(PositiveInteger.MinValue.Value))
                      .Should()
                      .BeOfType<ViewResult>()
                      .Which.Model.Should()
                      .BeAssignableTo<IEnumerable<BlogViewModel>>();
        }

        [Fact]
        public async Task PageShouldReturnBlogModelsWithAnAuxiliaryPageNumberWhenThereAreMoreThanTenPages()
        {
            var blogService = Substitute.For<IBlogService>();
            blogService.GetOrderedByDatesAsync(Arg.Any<int>(), Arg.Any<int>())
                       .Returns(new[] { new Blog(string.Empty, string.Empty) });
            blogService.GetCountAsync(Arg.Any<int>()).Returns(101);
            var controller = GetPostsController(blogService);

            (await controller.Page(PositiveInteger.MinValue.Value))
                      .Should()
                      .BeOfType<ViewResult>()
                      .Which.Model.Should()
                      .BeAssignableTo<Pager<BlogViewModel>>()
                      .Which.AuxiliaryPage.Should()
                      .Be(11.Some());
        }

        [Fact]
        public async Task PageShouldReturnBlogModelsWithoutAnAuxiliaryPageNumberWhenThereAreLessThanElevenItems()
        {
            var blogs = new[]
            {
                new Blog(321, default, default,
                    new Dates(new DateTime(2014, 1, 1), Option.None<DateTime>()))
            };

            var blogService = Substitute.For<IBlogService>();
            blogService.GetOrderedByDatesAsync(Arg.Any<int>(), Arg.Any<int>()).Returns(blogs);
            blogService.GetCountAsync(Arg.Any<int>()).Returns(1);
            var controller = GetPostsController(blogService);

            (await controller.Page(1))
                             .Should()
                             .BeOfType<ViewResult>()
                             .Which.Model.Should()
                             .BeAssignableTo<Pager<BlogViewModel>>()
                             .Which.AuxiliaryPage.Should()
                             .Be(Option.None<int>());
        }

        [Fact]
        public async Task PageShouldReturnBlogModelsWithValuesSetFromTheBlogService()
        {
            var blog = new Blog(123,
                "title",
                "content",
                new Dates(new DateTime(2015, 7, 27), Option.Some(new DateTime(2016, 8, 28))));

            var blogService = Substitute.For<IBlogService>();
            blogService.GetOrderedByDatesAsync(Arg.Any<int>(), Arg.Any<int>()).Returns(new[] { blog });
            blogService.GetCountAsync(1).Returns(int.MaxValue);

            var controller = GetPostsController(blogService);

            (await controller.Page(PositiveInteger.MinValue.Value))
                             .Should()
                             .BeOfType<ViewResult>()
                             .Which.Model.Should()
                             .BeEquivalentTo(
                                 new[]
                                 {
                                     new
                                     {
                                         blog.Id, blog.Title, blog.Content,
                                         Dates = new { blog.Dates.Created, blog.Dates.Modified }
                                     }
                                 });
        }

        [Fact]
        public async Task PageShouldReturnNotFoundResultWhenNoBlogsFound()
        {
            var blogService = Substitute.For<IBlogService>();
            (await GetPostsController(blogService).Page(1)).Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task PageShouldReturnPageView()
        {
            var blogService = Substitute.For<IBlogService>();
            blogService.GetOrderedByDatesAsync(Arg.Any<int>(), Arg.Any<int>())
                       .Returns(new[] { new Blog(string.Empty, string.Empty) });
            blogService.GetCountAsync(Arg.Any<int>()).Returns(101);
            var controller = GetPostsController(blogService);

            (await controller.Page(PositiveInteger.MinValue.Value)).Should().BeOfType<ViewResult>();
        }
    }
}
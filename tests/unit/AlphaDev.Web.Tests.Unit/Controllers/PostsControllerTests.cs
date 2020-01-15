using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AlphaDev.BlogServices.Core;
using AlphaDev.Core;
using AlphaDev.Paging;
using AlphaDev.Paging.Extensions;
using AlphaDev.Services;
using AlphaDev.Web.Controllers;
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
            blogService.GetCountAsync().Returns(1);
            return GetPostsController(blogService, PagesSettings.Default);
        }

        [NotNull]
        private PostsController GetPostsController([NotNull] IBlogService blogService, PagesSettings pagesSettings) =>
            new PostsController(blogService, pagesSettings);

        [Fact]
        public void CreateShouldReturnCreateView()
        {
            var controller = GetPostsController();

            controller.Create().ViewName.Should().BeEquivalentTo("Create");
        }

        [Fact]
        public async Task CreateShouldReturnCreateViewWhenModelStateIsInvalid()
        {
            var controller = GetPostsController(Substitute.For<IBlogService>(), PagesSettings.Default);

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
            var controller = GetPostsController(Substitute.For<IBlogService>(), PagesSettings.Default);

            controller.ModelState.AddModelError("test", "test");

            var post = new CreatePostViewModel("title", "content");

            (await controller.Create(post)).Should().BeOfType<ViewResult>().Which.Model.Should().BeEquivalentTo(post);
        }

        [Fact]
        public async Task CreateShouldRouteIdArgument()
        {
            var controller = GetPostsController(Substitute.For<IBlogService>(), PagesSettings.Default);

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
            var controller = GetPostsController(Substitute.For<IBlogService>(), PagesSettings.Default);

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
            var controller = GetPostsController(service, PagesSettings.Default);

            const int id = 10;
            await controller.Delete(id);

            await service.Received().DeleteAsync(id);
        }

        [Fact]
        public async Task DeleteShouldRedirectToDefaultPageActionWhenDeletedSuccessfully()
        {
            var service = Substitute.For<IBlogService>();
            service.DeleteAsync(1)
                   .Returns(Option.Some<BlogServices.Core.Unit, ObjectNotFoundException<BlogBase>>(
                       BlogServices.Core.Unit.Value));
            var controller = GetPostsController(service, PagesSettings.Default);

            var result = (await controller.Delete(1)).Should().BeOfType<RedirectToActionResult>();
            result.Which.ActionName.Should().BeEquivalentTo("Page");
            result.Which.RouteValues["id"].Should().BeNull();
        }

        [Fact]
        public async Task DeleteShouldReturnNotFoundWhenDeleteWasUnsuccessful()
        {
            var service = Substitute.For<IBlogService>();
            service.DeleteAsync(1)
                   .Returns(Option.None<BlogServices.Core.Unit, ObjectNotFoundException<BlogBase>>(
                       new ObjectNotFoundException<BlogBase>()));
            var controller = GetPostsController(service, PagesSettings.Default);
            (await controller.Delete(1)).Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task EditShouldEditPostWithModelData()
        {
            var blogService = Substitute.For<IBlogService>();
            var editArguments = new BlogEditArguments();

            await blogService.EditAsync(1, Arg.Invoke(editArguments));
            var controller = GetPostsController(blogService, PagesSettings.Default);

            var post = new EditPostViewModel("title", "content", default);
            await controller.Edit(1, post);

            editArguments.Should().BeEquivalentTo(new { post.Content, post.Title });
        }

        [Fact]
        public async Task EditShouldNotFoundResultWhenBlogIsNotFound()
        {
            var service = Substitute.For<IBlogService>();
            service.EditAsync(Arg.Any<int>(), Arg.Any<Action<BlogEditArguments>>())
                   .Returns(Option.None<BlogServices.Core.Unit, ObjectNotFoundException<BlogBase>>(
                       new ObjectNotFoundException<BlogBase>()));

            var controller = GetPostsController(service, PagesSettings.Default);

            (await controller.Edit(default, new EditPostViewModel(string.Empty, string.Empty, default)))
                .Should()
                .BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task EditShouldReturnEditViewWhenBlogIsFound()
        {
            var blogService = Substitute.For<IBlogService>();
            blogService.GetAsync(Arg.Any<int>()).Returns(Option.Some(Substitute.For<BlogBase>()));

            var controller = GetPostsController(blogService, PagesSettings.Default);

            (await controller.Edit(default))
                .Should()
                .BeOfType<ViewResult>()
                .Which.ViewName.Should()
                .BeEquivalentTo("Edit");
        }

        [Fact]
        public async Task EditShouldReturnEditViewWhenModelStateIsInvalid()
        {
            var controller = GetPostsController(Substitute.For<IBlogService>(), PagesSettings.Default);

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

            var controller = GetPostsController(blogService, PagesSettings.Default);

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
            var service = Substitute.For<IBlogService>();
            service.EditAsync(1, Arg.Any<Action<BlogEditArguments>>())
                   .Returns(Option.None<BlogServices.Core.Unit, ObjectNotFoundException<BlogBase>>(
                       new ObjectNotFoundException<BlogBase>()));
            var controller = GetPostsController(service, PagesSettings.Default);

            controller.ModelState.AddModelError("test", "test");

            var post = new EditPostViewModel("title", "content", new DatesViewModel());

            (await controller.Edit(1, post))
                .Should()
                .BeOfType<ViewResult>()
                .Which.Model.Should()
                .BeEquivalentTo(post);
        }

        [Fact]
        public async Task EditShouldRouteToIndexActionWhenModelStateIsValidAndEditIsSuccessful()
        {
            var service = Substitute.For<IBlogService>();
            service.EditAsync(Arg.Any<int>(), Arg.Any<Action<BlogEditArguments>>())
                   .Returns(Option.Some<BlogServices.Core.Unit, ObjectNotFoundException<BlogBase>>(
                       BlogServices.Core.Unit.Value));

            var controller = GetPostsController(service, PagesSettings.Default);

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
            var controller = GetPostsController(blogService, PagesSettings.Default);

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

            var controller = GetPostsController(blogService, PagesSettings.Default);

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

            var controller = GetPostsController(blogService, PagesSettings.Default);

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

            var controller = GetPostsController(blogService, PagesSettings.Default);

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
            const int page = 9;
            blogService.GetOrderedByDatesWithTotalAsync(Arg.Any<int>(), Arg.Any<int>()).Returns((81, Enumerable.Empty<BlogBase>()));
            await GetPostsController(blogService, PagesSettings.Default).Page(page);
            var value = (page - 1) * 10 + 1;
            await blogService.Received(1).GetOrderedByDatesWithTotalAsync(value, 10);
        }

        [Fact]
        public async Task PageShouldReturnBlogModelsAssignableToEnumerableOfBlogViewModel()
        {
            var blogService = Substitute.For<IBlogService>();
            blogService.GetOrderedByDatesWithTotalAsync(Arg.Any<int>(), Arg.Any<int>())
                       .Returns((1, new[] { new Blog(string.Empty, string.Empty) }));
            blogService.GetCountAsync().Returns(1);
            var controller = GetPostsController(blogService, PagesSettings.Default);
            (await controller.Page(1))
                .Should()
                .BeOfType<ViewResult>()
                .Which.Model.Should()
                .BeAssignableTo<IEnumerable<BlogViewModel>>();
        }

        [Fact]
        public async Task PageShouldReturnBlogModelsWithValuesSetFromTheBlogService()
        {
            var blog = new Blog(123,
                "title",
                "content",
                new Dates(new DateTime(2015, 7, 27), Option.Some(new DateTime(2016, 8, 28))));

            var blogService = Substitute.For<IBlogService>();
            blogService.GetOrderedByDatesWithTotalAsync(Arg.Any<int>(), Arg.Any<int>()).Returns((100, new[] { blog }));

            var controller = GetPostsController(blogService, PagesSettings.Default);

            (await controller.Page(10))
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
            blogService.GetOrderedByDatesWithTotalAsync(Arg.Any<int>(), Arg.Any<int>())
                       .Returns((default, Array.Empty<BlogBase>()));
            (await GetPostsController(blogService, PagesSettings.Default).Page(1)).Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task PageShouldReturnPageView()
        {
            var blogService = Substitute.For<IBlogService>();
            blogService.GetOrderedByDatesWithTotalAsync(Arg.Any<int>(), Arg.Any<int>())
                       .Returns((100, new[] { new Blog(string.Empty, string.Empty) }));
            var controller = GetPostsController(blogService, PagesSettings.Default);

            (await controller.Page(10)).Should().BeOfType<ViewResult>();
        }
    }
}
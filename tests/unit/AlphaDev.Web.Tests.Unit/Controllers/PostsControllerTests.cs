using System;
using System.Collections.Generic;
using AlphaDev.Core;
using AlphaDev.Core.Extensions;
using AlphaDev.Web.Controllers;
using AlphaDev.Web.Core.Extensions;
using AlphaDev.Web.Core.Support;
using AlphaDev.Web.Models;
using AlphaDev.Web.Support;
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
            blogService.GetLatest().Returns(((BlogBase) new Blog(default, null, null, default)).Some());
            blogService.GetCount(Arg.Any<int>()).Returns(1);
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
        public void CreateShouldReturnCreateViewWhenModelStateIsInvalid()
        {
            var controller = GetPostsController(Substitute.For<IBlogService>());

            controller.ModelState.AddModelError("test", "test");

            controller.Create(default)
                      .Should()
                      .BeOfType<ViewResult>()
                      .Which.ViewName.Should()
                      .BeEquivalentTo("Create");
        }

        [Fact]
        public void CreateShouldReturnViewWithSameModelWhenModelStateIsInvalid()
        {
            var controller = GetPostsController(Substitute.For<IBlogService>());

            controller.ModelState.AddModelError("test", "test");

            var post = new CreatePostViewModel("title", "content");

            controller.Create(post).Should().BeOfType<ViewResult>().Which.Model.Should().BeEquivalentTo(post);
        }

        [Fact]
        public void CreateShouldRouteIdArgument()
        {
            var controller = GetPostsController(Substitute.For<IBlogService>());

            controller.TempData = new TempDataDictionary(new DefaultHttpContext(), Substitute.For<ITempDataProvider>());

            var post = new CreatePostViewModel("title", "content");

            controller.Create(post)
                      .Should()
                      .BeOfType<RedirectToActionResult>()
                      .Which.RouteValues.Should()
                      .ContainKey("id")
                      .WhichValue.Should()
                      .BeEquivalentTo(default(int));
        }

        [Fact]
        public void CreateShouldRouteToIndexAction()
        {
            var controller = GetPostsController(Substitute.For<IBlogService>());

            controller.TempData = new TempDataDictionary(new DefaultHttpContext(), Substitute.For<ITempDataProvider>());

            var post = new CreatePostViewModel("title", "content");

            controller.Create(post)
                      .Should()
                      .BeOfType<RedirectToActionResult>()
                      .Which.ActionName.Should()
                      .BeEquivalentTo("Index");
        }

        [Fact]
        public void DeleteShouldDeleteBlogWithMatchingId()
        {
            var service = Substitute.For<IBlogService>();
            var controller = GetPostsController(service);

            const int id = 10;
            controller.Delete(id);

            service.Received().Delete(id);
        }

        [Fact]
        public void DeleteShouldRedirectToDefaultPageAction()
        {
            var controller = GetPostsController(Substitute.For<IBlogService>());

            var result = controller.Delete(default).Should().BeOfType<RedirectToActionResult>();
            result.Which.ActionName.Should().BeEquivalentTo("Page");
            result.Which.RouteValues["id"].Should().BeNull();
        }

        [Fact]
        public void EditShouldEditPostWithModelData()
        {
            var blogService = Substitute.For<IBlogService>();
            var editArguments = new BlogEditArguments();

            blogService.Edit(1, Arg.Invoke(editArguments));
            var controller = GetPostsController(blogService);

            var post = new EditPostViewModel("title", "content", default);
            controller.Edit(1, post);

            editArguments.Should().BeEquivalentTo(new { post.Content, post.Title });
        }

        [Fact]
        public void EditShouldReturnEditViewWhenBlogIsFound()
        {
            var blogService = Substitute.For<IBlogService>();
            blogService.Get(Arg.Any<int>()).Returns(Option.Some(Substitute.For<BlogBase>()));

            var controller = GetPostsController(blogService);

            controller.Edit(default)
                      .Should()
                      .BeOfType<ViewResult>()
                      .Which.ViewName.Should()
                      .BeEquivalentTo("Edit");
        }

        [Fact]
        public void EditShouldReturnEditViewWhenModelStateIsInvalid()
        {
            var controller = GetPostsController(Substitute.For<IBlogService>());

            controller.ModelState.AddModelError("test", "test");

            var post = new EditPostViewModel("title", "content", new DatesViewModel());

            controller.Edit(default, post)
                      .Should()
                      .BeOfType<ViewResult>()
                      .Which.ViewName.Should()
                      .BeEquivalentTo("Edit");
        }

        [Fact]
        public void EditShouldReturnEditViewWithModelWhenBlogIsFound()
        {
            var blog = Substitute.For<BlogBase>();
            blog.Id.Returns(default(int));
            blog.Content.Returns("content");
            blog.Title.Returns("title");
            blog.Dates.Returns(default(Dates));

            var blogService = Substitute.For<IBlogService>();
            blogService.Get(Arg.Any<int>()).Returns(Option.Some(blog));

            var controller = GetPostsController(blogService);

            controller
                .Edit(default)
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
        public void EditShouldReturnViewWithSameModelWhenModelStateIsInvalid()
        {
            var controller = GetPostsController(Substitute.For<IBlogService>());

            controller.ModelState.AddModelError("test", "test");

            var post = new EditPostViewModel("title", "content", new DatesViewModel());

            controller.Edit(default, post).Should().BeOfType<ViewResult>().Which.Model.Should().BeEquivalentTo(post);
        }

        [Fact]
        public void EditShouldRouteToIndexActionWhenModelStateIsValid()
        {
            var controller = GetPostsController(Substitute.For<IBlogService>());

            controller.Edit(default, new EditPostViewModel(string.Empty, string.Empty, default))
                      .Should()
                      .BeOfType<RedirectToActionResult>()
                      .Which.ActionName.Should()
                      .BeEquivalentTo("Index");
        }

        [Fact]
        public void IndexShouldReturn404StatusIfNoBlogFound()
        {
            var blogService = Substitute.For<IBlogService>();
            var controller = GetPostsController(blogService);

            blogService.Get(Arg.Any<int>()).Returns(Option.None<BlogBase>());

            controller.Index(default).Should().BeOfType<NotFoundResult>().Which.StatusCode.Should().Be(404);
        }

        [Fact]
        public void IndexShouldReturnBlogModelWithValuesSetFromTheBlogServiceWhenTempDataOrModelDontExist()
        {
            const int id = 123;
            BlogBase blog = new Blog(id,
                "title",
                "content",
                new Dates(new DateTime(2015, 7, 27), Option.Some(new DateTime(2016, 8, 28))));

            var blogService = Substitute.For<IBlogService>();
            blogService.Get(id).Returns(blog.Some());

            var controller = GetPostsController(blogService);

            controller.Index(id)
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
        public void IndexShouldReturnPostViewWhenPostIsFound()
        {
            BlogBase blog = new Blog(123,
                "title",
                "content",
                new Dates(new DateTime(2015, 7, 27), Option.Some(new DateTime(2016, 8, 28))));

            var blogService = Substitute.For<IBlogService>();
            blogService.Get(Arg.Any<int>()).Returns(Option.Some(blog));

            var controller = GetPostsController(blogService);

            controller.Index(default).Should().BeOfType<ViewResult>();
        }

        [Fact]
        public void IndexShouldReturnPostViewWithBlogTitleAsPageTitle()
        {
            const int id = 123;
            BlogBase blog = new Blog(id,
                "title",
                "content",
                new Dates(new DateTime(2015, 7, 27), Option.Some(new DateTime(2016, 8, 28))));

            var blogService = Substitute.For<IBlogService>();
            blogService.Get(id).Returns(Option.Some(blog));

            var controller = GetPostsController(blogService);

            controller.Index(id)
                      .Should()
                      .BeOfType<ViewResult>()
                      .Which.ViewData["Title"]
                      .Should()
                      .BeEquivalentTo("title");
        }

        [Fact]
        public void PageShouldGetLessThanElevenItems()
        {
            var blogService = Substitute.For<IBlogService>();
            blogService.GetCount(Arg.Any<int>()).Returns(1);
            const int page = 9;
            GetPostsController(blogService).Page(page);
            var value = page.ToPositiveInteger().ToStartPosition(10.ToPositiveInteger()).Value;
            blogService.Received(1).GetOrderedByDates(Arg.Is(value), Arg.Is(10));
        }

        [Fact]
        public void PageShouldReturnBlogModelsAssignableToEnumerableOfBlogViewModel()
        {
            var blogService = Substitute.For<IBlogService>();
            blogService.GetOrderedByDates(Arg.Any<int>(), Arg.Any<int>())
                       .Returns(new[] { new Blog(string.Empty, string.Empty) });
            blogService.GetCount(Arg.Any<int>()).Returns(1);
            var controller = GetPostsController(blogService);
            controller.Page(PositiveInteger.MinValue.Value)
                      .Should()
                      .BeOfType<ViewResult>()
                      .Which.Model.Should()
                      .BeAssignableTo<IEnumerable<BlogViewModel>>();
        }

        [Fact]
        public void PageShouldReturnBlogModelsWithAnAuxiliaryPageNumberWhenThereAreMoreThanTenPages()
        {
            var blogService = Substitute.For<IBlogService>();
            blogService.GetOrderedByDates(Arg.Any<int>(), Arg.Any<int>())
                       .Returns(new[] { new Blog(string.Empty, string.Empty) });
            blogService.GetCount(Arg.Any<int>()).Returns(101);
            var controller = GetPostsController(blogService);

            controller.Page(PositiveInteger.MinValue.Value)
                      .Should()
                      .BeOfType<ViewResult>()
                      .Which.Model.Should()
                      .BeAssignableTo<Pager<BlogViewModel>>()
                      .Which.AuxiliaryPage.Should()
                      .Be(11.Some());
        }

        [Fact]
        public void PageShouldReturnBlogModelsWithoutAnAuxiliaryPageNumberWhenThereAreLessThanElevenItems()
        {
            var blogs = new[]
            {
                new Blog(321, default, default,
                    new Dates(new DateTime(2014, 1, 1), Option.None<DateTime>()))
            };

            var blogService = Substitute.For<IBlogService>();
            blogService.GetOrderedByDates(Arg.Any<int>(), Arg.Any<int>()).Returns(blogs);
            blogService.GetCount(Arg.Any<int>()).Returns(1);
            var controller = GetPostsController(blogService);

            controller.Page(1)
                      .Should()
                      .BeOfType<ViewResult>()
                      .Which.Model.Should()
                      .BeAssignableTo<Pager<BlogViewModel>>()
                      .Which.AuxiliaryPage.Should()
                      .Be(Option.None<int>());
        }

        [Fact]
        public void PageShouldReturnBlogModelsWithValuesSetFromTheBlogService()
        {
            var blog = new Blog(123,
                "title",
                "content",
                new Dates(new DateTime(2015, 7, 27), Option.Some(new DateTime(2016, 8, 28))));

            var blogService = Substitute.For<IBlogService>();
            blogService.GetOrderedByDates(Arg.Any<int>(), Arg.Any<int>()).Returns(new[] { blog });
            blogService.GetCount(1).Returns(int.MaxValue);

            var controller = GetPostsController(blogService);

            controller.Page(PositiveInteger.MinValue.Value)
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
        public void PageShouldReturnNotFoundResultWhenNoBlogsFound()
        {
            var blogService = Substitute.For<IBlogService>();
            GetPostsController(blogService).Page(1).Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public void PageShouldReturnPageView()
        {
            var blogService = Substitute.For<IBlogService>();
            blogService.GetOrderedByDates(Arg.Any<int>(), Arg.Any<int>())
                       .Returns(new[] { new Blog(string.Empty, string.Empty) });
            blogService.GetCount(Arg.Any<int>()).Returns(101);
            var controller = GetPostsController(blogService);

            controller.Page(PositiveInteger.MinValue.Value).Should().BeOfType<ViewResult>();
        }
    }
}
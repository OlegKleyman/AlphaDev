using System;
using System.Collections.Generic;
using System.Linq;
using AlphaDev.Core;
using AlphaDev.Web.Controllers;
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
using Int32 = AlphaDev.Web.Extensions.Int32;
using PositiveInteger = AlphaDev.Web.Extensions.PositiveInteger;

namespace AlphaDev.Web.Tests.Unit.Controllers
{
    public class PostsControllerTests
    {
        [NotNull]
        private PostsController GetPostsController()
        {
            var blogService = Substitute.For<IBlogService>();
            blogService.GetLatest().Returns(((BlogBase) new Blog(default, null, null, default)).Some());

            return GetPostsController(blogService);
        }

        [NotNull]
        private PostsController GetPostsController([NotNull] IBlogService blogService)
        {
            return new PostsController(blogService);
        }

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

            controller.Create(default).Should().BeOfType<ViewResult>().Which.ViewName.Should()
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

            controller.Create(post).Should().BeOfType<RedirectToActionResult>().Which.RouteValues.Should()
                .ContainKey("id").WhichValue.Should().BeEquivalentTo(default(int));
        }

        [Fact]
        public void CreateShouldRouteToIndexAction()
        {
            var controller = GetPostsController(Substitute.For<IBlogService>());

            controller.TempData = new TempDataDictionary(new DefaultHttpContext(), Substitute.For<ITempDataProvider>());

            var post = new CreatePostViewModel("title", "content");

            controller.Create(post).Should().BeOfType<RedirectToActionResult>().Which.ActionName.Should()
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
                .Which.ViewName.Should().BeEquivalentTo("Edit");
        }

        [Fact]
        public void EditShouldReturnEditViewWhenModelStateIsInvalid()
        {
            var controller = GetPostsController(Substitute.For<IBlogService>());

            controller.ModelState.AddModelError("test", "test");

            var post = new EditPostViewModel("title", "content", new DatesViewModel());

            controller.Edit(default, post).Should().BeOfType<ViewResult>().Which.ViewName.Should()
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
        public void EditShouldRouteToIndexAction()
        {
            var controller = GetPostsController(Substitute.For<IBlogService>());

            controller.Edit(default, Arg.Any<EditPostViewModel>())
                .Should().BeOfType<RedirectToActionResult>()
                .Which.ActionName.Should().BeEquivalentTo("Index");
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

            controller.Page(Web.Support.PositiveInteger.MinValue.Value).Model.Should().BeEquivalentTo(
                new[]
                {
                    new { blog.Id, blog.Title, blog.Content, Dates = new { blog.Dates.Created, blog.Dates.Modified } }
                });
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

            controller.Index(id).Should().BeOfType<ViewResult>().Which.Model.Should().BeEquivalentTo(
                new { blog.Id, blog.Title, blog.Content, Dates = new { blog.Dates.Created, blog.Dates.Modified } });
        }

        [Fact]
        public void PageShouldReturnPageView()
        {
            var controller = GetPostsController();

            controller.Page(Web.Support.PositiveInteger.MinValue.Value).Should().BeOfType<ViewResult>();
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

            controller.Index(id).Should().BeOfType<ViewResult>().Which.ViewData["Title"].Should()
                .BeEquivalentTo("title");
        }

        [Fact]
        public void PageShouldReturnBlogModelsAssignableToEnumerableOfBlogViewModel()
        {
            var blogService = Substitute.For<IBlogService>();
            blogService.GetOrderedByDates(Arg.Any<int>(), Arg.Any<int>()).Returns(Enumerable.Empty<Blog>());
            var controller = GetPostsController(blogService);
            controller.Page(Web.Support.PositiveInteger.MinValue.Value).Model.Should().BeAssignableTo<IEnumerable<BlogViewModel>>();
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

            var controller = GetPostsController(blogService);

            controller.Page(1).Model.Should().BeAssignableTo<Pager<BlogViewModel>>().Which.AuxiliaryPage.Should()
                .Be(Option.None<int>());
        }

        [Fact]
        public void PageShouldReturnBlogModelsWithAnAuxiliaryPageNumberWhenThereAreMoreThanTenPages()
        {
            var blogService = Substitute.For<IBlogService>();
            blogService.GetOrderedByDates(Arg.Any<int>(), Arg.Any<int>()).Returns(Array.Empty<BlogBase>());
            blogService.GetCount(Arg.Any<int>()).Returns(101);
            var controller = GetPostsController(blogService);

            controller.Page(Web.Support.PositiveInteger.MinValue.Value).Model.Should().BeAssignableTo<Pager<BlogViewModel>>().Which.AuxiliaryPage.Should()
                .Be(11.Some());
        }

        [Fact]
        public void PageShouldGetLessThanElevenItems()
        {
            var blogService = Substitute.For<IBlogService>();
            const int page = 9;
            GetPostsController(blogService).Page(page);
            var value = PositiveInteger.ToStartPosition(Int32.ToPositiveInteger(page), Int32.ToPositiveInteger(10)).Value;
            blogService.Received(1).GetOrderedByDates(Arg.Is(value), Arg.Is(10));
        }
    }
}
using System;
using System.Collections.Generic;
using AlphaDev.Core;
using AlphaDev.Web.Controllers;
using AlphaDev.Web.Models;
using AlphaDev.Web.Support;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Newtonsoft.Json;
using NSubstitute;
using Optional;
using Xunit;

namespace AlphaDev.Web.Tests.Unit.Controllers
{
    public class PostsControllerTests
    {
        private PostsController GetPostsController()
        {
            var blogService = Substitute.For<IBlogService>();
            blogService.GetLatest().Returns(((BlogBase) new Blog(default, null, null, default)).Some());

            return GetPostsController(blogService);
        }

        private PostsController GetPostsController(IBlogService blogService)
        {
            return new PostsController(blogService);
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
        public void CreateShouldSetTempDataWithModelFromDatastoreWhenTempDataIsNotNull()
        {
            var blogService = Substitute.For<IBlogService>();
            var blog = Substitute.For<BlogBase>();
            blog.Id.Returns(1);
            blog.Title.Returns("title");
            blog.Content.Returns("content");
            blog.Dates.Returns(new Dates(new DateTime(2000, 1, 1), Option.Some(new DateTime(2018, 3, 18))));

            blogService.Add(Arg.Any<BlogBase>()).Returns(blog);

            var controller = GetPostsController(blogService);

            controller.TempData = new TempDataDictionary(new DefaultHttpContext(), Substitute.For<ITempDataProvider>());

            var post = new CreatePostViewModel("title", "content");
            controller.Create(post);

            controller.TempData.Should().ContainKey("Model").WhichValue.Should().BeEquivalentTo(
                "{\"Id\":1,\"Title\":\"title\",\"Content\":\"content\",\"Dates\":{\"Created\":\"2000-01-01T00:00:00\",\"Modified\":\"2018-03-18T00:00:00\"}}");
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
        public void DeleteShouldRedirectToDefaultIndexAction()
        {
            var controller = GetPostsController(Substitute.For<IBlogService>());

            var result = controller.Delete(default).Should().BeOfType<RedirectToActionResult>();
            result.Which.ActionName.Should().BeEquivalentTo("Index");
            result.Which.RouteValues["id"].Should().BeNull();
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
        public void IndexShouldReturnBlogModelsAssignableToEnumerableOfBlogViewModel()
        {
            var blogs = new[]
            {
                new Blog(321, default, default,
                    new Dates(new DateTime(2014, 1, 1), Option.None<DateTime>())),
                new Blog(123,
                    "title",
                    "content",
                    new Dates(new DateTime(2015, 7, 27), Option.Some(new DateTime(2016, 8, 28))))
            };

            var blogService = Substitute.For<IBlogService>();
            blogService.GetAll().Returns(blogs);

            var controller = GetPostsController(blogService);

            controller.Index().Model.Should().BeAssignableTo<IEnumerable<BlogViewModel>>();
        }

        [Fact]
        public void IndexShouldReturnBlogModelsOrderedByCreatedDateDescending()
        {
            var blogs = new[]
            {
                new Blog(321, default, default,
                    new Dates(new DateTime(2014, 1, 1), Option.None<DateTime>())),
                new Blog(123,
                    "title",
                    "content",
                    new Dates(new DateTime(2015, 7, 27), Option.Some(new DateTime(2016, 8, 28))))
            };

            var blogService = Substitute.For<IBlogService>();
            blogService.GetAll().Returns(blogs);

            var controller = GetPostsController(blogService);

            controller.Index().Model.Should().BeAssignableTo<IEnumerable<BlogViewModel>>().Which.Should()
                .BeInDescendingOrder(model => model.Dates.Created);
        }

        [Fact]
        public void IndexShouldReturnBlogModelsWithValuesSetFromTheBlogService()
        {
            var blog = new Blog(123,
                "title",
                "content",
                new Dates(new DateTime(2015, 7, 27), Option.Some(new DateTime(2016, 8, 28))));

            var blogService = Substitute.For<IBlogService>();
            blogService.GetAll().Returns(new[] {blog});

            var controller = GetPostsController(blogService);

            controller.Index().Model.Should().BeEquivalentTo(
                new[] {new {blog.Id, blog.Title, blog.Content, Dates = new {blog.Dates.Created, blog.Dates.Modified}}});
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
                new {blog.Id, blog.Title, blog.Content, Dates = new {blog.Dates.Created, blog.Dates.Modified}});
        }

        [Fact]
        public void IndexShouldReturnIndexView()
        {
            var controller = GetPostsController();

            controller.Index().Should().BeOfType<ViewResult>();
        }

        [Fact]
        public void IndexShouldReturnPostViewModelFromSerializedTempDataWhenItExists()
        {
            var blog = new BlogViewModel(default,
                "title",
                "content",
                new DatesViewModel(new DateTime(2015, 7, 27), Option.Some(new DateTime(2016, 8, 28))));

            var controller = GetPostsController(Substitute.For<IBlogService>());
            controller.TempData = new TempDataDictionary(new DefaultHttpContext(), Substitute.For<ITempDataProvider>())
            {
                ["Model"] = JsonConvert.SerializeObject(blog, BlogViewModelConverter.Default)
            };

            controller.Index(default).Should().BeOfType<ViewResult>().Which.Model.Should().BeEquivalentTo(
                new {blog.Id, blog.Title, blog.Content, Dates = new {blog.Dates.Created, blog.Dates.Modified}});
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
        public void IndexShouldReturnTitleFromSerializedTempDataWhenItExists()
        {
            var blog = new BlogViewModel(default,
                "title",
                "content",
                new DatesViewModel(new DateTime(2015, 7, 27), Option.Some(new DateTime(2016, 8, 28))));

            var controller = GetPostsController(Substitute.For<IBlogService>());
            controller.TempData = new TempDataDictionary(new DefaultHttpContext(), Substitute.For<ITempDataProvider>())
            {
                ["Model"] = JsonConvert.SerializeObject(blog, BlogViewModelConverter.Default)
            };

            controller.Index(default).Should().BeOfType<ViewResult>().Which.ViewData["Title"].Should()
                .BeEquivalentTo(blog.Title);
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
        public void EditShouldReturnEditViewWhenModelStateIsInvalid()
        {
            var controller = GetPostsController(Substitute.For<IBlogService>());

            controller.ModelState.AddModelError("test", "test");

            var post = new EditPostViewModel("title", "content", new DatesViewModel());

            controller.Edit(default, post).Should().BeOfType<ViewResult>().Which.ViewName.Should()
                .BeEquivalentTo("Edit");
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
        public void EditShouldEditPostWithModelData()
        {
            var blogService = Substitute.For<IBlogService>();
            var editArguments = new BlogEditArguments();

            blogService.Edit(1, Arg.Invoke(editArguments));
            var controller = GetPostsController(blogService);

            var post = new EditPostViewModel("title", "content", default);
            controller.Edit(1, post);

            editArguments.Should().BeEquivalentTo(new {post.Content, post.Title});
        }
    }
}
using System;
using System.Threading.Tasks;
using AlphaDev.Web.Models;
using AlphaDev.Web.ViewComponents;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Optional;
using Xunit;

namespace AlphaDev.Web.Tests.Unit.ViewComponents
{
    public class BlogPreviewViewComponentTests
    {
        private BlogPreviewViewComponent BlogViewComponent()
        {
            return new BlogPreviewViewComponent();
        }

        [Fact]
        public async Task InvokeAsyncShouldReturnBlogViewComponentResult()
        {
            var sut = BlogViewComponent();

            var result = (ViewViewComponentResult) await sut.InvokeAsync(null);

            result.ViewName.ShouldBeEquivalentTo("BlogPreview");
        }

        [Fact]
        public async Task InvokeAsyncShouldReturnViewComponentResult()
        {
            var sut = BlogViewComponent();

            var result = await sut.InvokeAsync(null);

            result.Should().BeOfType<ViewViewComponentResult>();
        }

        [Fact]
        public async Task InvokeAsyncShouldReturnViewComponentResultWithBlogViewModel()
        {
            var sut = BlogViewComponent();

            var result = (ViewViewComponentResult) await sut.InvokeAsync(new BlogViewModel(default(int), default(string),
                default(string), new DatesViewModel(default(DateTime), Option.None<DateTime>())));

            result.ViewData.Model.Should()
                .BeOfType<BlogViewModel>();
        }

        [Fact]
        public async Task InvokeAsyncShouldReturnViewComponentResultWithBlogiewModelWithThePassedBlog()
        {
            var sut = BlogViewComponent();

            var blogViewModel = new BlogViewModel(default (int), default(string), default(string), new DatesViewModel(default(DateTime), Option.None<DateTime>()));
            var result = (BlogViewModel) ((ViewViewComponentResult) await sut.InvokeAsync(blogViewModel)).ViewData
                .Model;
            result.Should().BeSameAs(blogViewModel);
        }
    }
}
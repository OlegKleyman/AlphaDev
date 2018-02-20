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
        private BlogPreviewViewComponent GetBlogPreviewViewComponent()
        {
            return new BlogPreviewViewComponent();
        }

        [Fact]
        public async Task InvokeAsyncShouldReturnBlogViewComponentResult()
        {
            var sut = GetBlogPreviewViewComponent();

            var result = (ViewViewComponentResult) await sut.InvokeAsync(null);

            result.ViewName.Should().BeEquivalentTo("BlogPreview");
        }

        [Fact]
        public async Task InvokeAsyncShouldReturnViewComponentResult()
        {
            var sut = GetBlogPreviewViewComponent();

            var result = await sut.InvokeAsync(null);

            result.Should().BeOfType<ViewViewComponentResult>();
        }

        [Fact]
        public async Task InvokeAsyncShouldReturnViewComponentResultWithBlogViewModel()
        {
            var sut = GetBlogPreviewViewComponent();

            var result = (ViewViewComponentResult) await sut.InvokeAsync(new BlogViewModel(default,
                default,
                default, new DatesViewModel(default, Option.None<DateTime>())));

            result.ViewData.Model.Should()
                .BeOfType<BlogViewModel>();
        }

        [Fact]
        public async Task InvokeAsyncShouldReturnViewComponentResultWithBlogViewModelWithThePassedBlog()
        {
            var sut = GetBlogPreviewViewComponent();

            var blogViewModel = new BlogViewModel(default, default, default,
                new DatesViewModel(default, Option.None<DateTime>()));
            var result = (BlogViewModel) ((ViewViewComponentResult) await sut.InvokeAsync(blogViewModel)).ViewData
                .Model;
            result.Should().BeSameAs(blogViewModel);
        }
    }
}
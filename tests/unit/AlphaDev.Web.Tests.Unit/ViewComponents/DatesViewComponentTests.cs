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
    public class DatesViewComponentTests
    {
        private DatesViewComponent GetDatesViewComponent()
        {
            return new DatesViewComponent();
        }

        [Fact]
        public async Task InvokeAsyncShouldReturnDatesViewComponentResult()
        {
            var sut = GetDatesViewComponent();

            var result = (ViewViewComponentResult) await sut.InvokeAsync(default, Option.None<DateTime>());

            result.ViewName.Should().BeEquivalentTo("Dates");
        }

        [Fact]
        public async Task InvokeAsyncShouldReturnViewComponentResult()
        {
            var sut = GetDatesViewComponent();

            var result = await sut.InvokeAsync(default, Option.None<DateTime>());

            result.Should().BeOfType<ViewViewComponentResult>();
        }

        [Fact]
        public async Task InvokeAsyncShouldReturnViewComponentResultWithDatesViewModel()
        {
            var sut = GetDatesViewComponent();

            var result = (ViewViewComponentResult) await sut.InvokeAsync(default, Option.None<DateTime>());
            result.ViewData.Model.Should()
                .BeOfType<DatesViewModel>();
        }

        [Fact]
        public async Task InvokeAsyncShouldReturnViewComponentResultWithDatesViewModelWithThePassedDates()
        {
            var sut = GetDatesViewComponent();

            var created = new DateTime(2015, 1, 27);
            var modified = Option.None<DateTime>();

            var result = (DatesViewModel) ((ViewViewComponentResult) await sut.InvokeAsync(created, modified)).ViewData
                .Model;
            result.Should().BeEquivalentTo(new {Created = created, Modified = modified});
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AlphaDev.Core.Data.Entities;
using AlphaDev.EntityFramework.Unit.Testing.Extensions;
using FluentAssertions;
using FluentAssertions.Optional;
using FluentAssertions.Optional.Extensions;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using MockQueryable.NSubstitute;
using NSubstitute;
using Xunit;

namespace AlphaDev.BlogServices.Tests.Unit
{
    public class AboutServiceTests
    {
        [NotNull]
        private AboutService GetAboutService(DbSet<About> abouts) => new AboutService(abouts);

        [Fact]
        public async Task CreateShouldSaveAboutToDataStore()
        {
            var aboutsDbSet = new List<About>().AsQueryable().BuildMockDbSet();
            var service = GetAboutService(aboutsDbSet);
            await service.CreateAsync("test");
            await aboutsDbSet.Received(1).AddAsync(Arg.Is<About>(about => about.Value == "test"));
        }

        [Fact]
        public async Task EditShouldEditAboutValue()
        {
            var abouts = new[] { new About() }.ToMockDbSet();
            var service = GetAboutService(abouts);
            await service.EditAsync("new value");
            ((string) abouts.Single().Value).Should().BeEquivalentTo("new value");
        }

        [Fact]
        public void EditShouldThrowInvalidOperationExceptionWhenAboutWasNotFound()
        {
            var abouts = new About[0].ToMockDbSet();
            var service = GetAboutService(abouts);
            Func<Task> edit = () => service.EditAsync(string.Empty);
            edit.Should().Throw<InvalidOperationException>().WithMessage("About not found.");
        }

        [Fact]
        public async Task GetAboutDetailsShouldReturnAboutDetails()
        {
            var abouts = new[] { new About { Value = "test" } }.ToMockDbSet();
            var service = GetAboutService(abouts);
            (await service.GetAboutDetailsAsync()).Should().HaveSome().Which.Should().BeEquivalentTo("test");
        }

        [Fact]
        public async Task GetAboutDetailsShouldReturnNoneWhenAboutIsNull()
        {
            var abouts = new List<About>().ToMockDbSet();
            var service = GetAboutService(abouts);
            (await service.GetAboutDetailsAsync()).Should().BeNone();
        }
    }
}
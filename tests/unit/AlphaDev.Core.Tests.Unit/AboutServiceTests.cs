﻿using System;
using System.Collections.Generic;
using System.Linq;
using AlphaDev.Core.Data.Entities;
using AlphaDev.EntityFramework.Unit.Testing.Extensions;
using AlphaDev.Test.Core.Extensions;
using FluentAssertions;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using MockQueryable.NSubstitute;
using NSubstitute;
using NSubstitute.ReceivedExtensions;
using Optional;
using Xunit;

namespace AlphaDev.Core.Tests.Unit
{
    public class AboutServiceTests
    {
        [NotNull]
        private AboutService GetAboutService(DbSet<About> abouts) => new AboutService(abouts);

        [Fact]
        public void CreateShouldSaveAboutToDataStore()
        {
            var aboutsDbSet = new List<About>().AsQueryable().BuildMockDbSet();
            var service = GetAboutService(aboutsDbSet);
            service.Create("test");
            aboutsDbSet.Received(1).Add(Arg.Is<About>(about => about.Value == "test"));
        }

        [Fact]
        public void EditShouldEditAboutValue()
        {
            var abouts = new[] { new About() }.ToMockDbSet();
            var service = GetAboutService(abouts);
            service.Edit("new value");
            abouts.Single().Value.Should().BeEquivalentTo("new value");
        }

        [Fact]
        public void EditShouldThrowInvalidOperationExceptionWhenAboutWasNotFound()
        {
            var abouts = new About[0].ToMockDbSet();
            var service = GetAboutService(abouts);
            Action edit = () => service.Edit(string.Empty);
            edit.Should().Throw<InvalidOperationException>().WithMessage("About not found.");
        }

        [Fact]
        public void GetAboutDetailsShouldReturnAboutDetails()
        {
            var abouts = new[] { new About { Value = "test" } }.ToMockDbSet();
            var service = GetAboutService(abouts);
            service.GetAboutDetails().Should().BeEquivalentTo(Option.Some("test"));
        }

        [Fact]
        public void GetAboutDetailsShouldReturnNoneWhenAboutIsNull()
        {
            var abouts = new List<About>().ToMockDbSet();
            var service = GetAboutService(abouts);
            service.GetAboutDetails().Should().BeEquivalentTo(Option.None<string>());
        }

        [Fact]
        public void GetAboutDetailsShouldReturnNoneWhenAboutValueIsNull()
        {
            var abouts = new[] { new About() }.ToMockDbSet();
            var service = GetAboutService(abouts);
            service.GetAboutDetails().Should().BeEquivalentTo(Option.None<string>());
        }
    }
}
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using AlphaDev.Core.Data;
using AlphaDev.Core.Data.Contexts;
using AlphaDev.Core.Data.Entities;
using AlphaDev.Core.Data.Support;
using AlphaDev.Core.Tests.Unit.Extensions.Support;
using AlphaDev.Test.Core.Extensions;
using FluentAssertions;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using NSubstitute;
using Optional;
using Xunit;

namespace AlphaDev.Core.Tests.Unit
{
    public class AboutServiceTests
    {
        [NotNull]
        private AboutService GetAboutService(InformationContext context)
        {
            var contextBuilder = Substitute.For<IContextFactory<InformationContext>>();
            contextBuilder.Create().Returns(context);
            return new AboutService(contextBuilder);
        }

        [Fact]
        public void CreateShouldSaveAboutToDataStore()
        {
            var context = Substitute.For<InformationContext>(Substitute.For<Configurer>()).Mock();
            var abouts = new List<About>();
            context.Abouts = abouts.ToMockDbSet().WithAddReturns(abouts);
            context.SaveChanges().Returns(1);
            var service = GetAboutService(context);
            service.Create("test");
            context.Abouts.Should().HaveCount(1).And.BeEquivalentTo(new { Value = "test" });
        }

        [Fact]
        public void CreateShouldSaveToDataStore()
        {
            var context = Substitute.For<InformationContext>(Substitute.For<Configurer>()).Mock();
            context.Abouts = new List<About>().ToMockDbSet();
            var entry = (EntityEntry<About>) FormatterServices.GetUninitializedObject(typeof(EntityEntry<About>));
            context.Abouts.Add(Arg.Any<About>()).Returns(entry);
            context.SaveChanges().Returns(1);
            var service = GetAboutService(context);
            service.Create("test");
            context.Received(1).SaveChanges();
        }

        [Fact]
        public void CreateShouldThrowInvalidOperationExceptionWhenUnableSavingReturnsUnexpectedSaveCount()
        {
            var context = Substitute.For<InformationContext>(Substitute.For<Configurer>()).Mock();
            context.Abouts = new List<About>().ToMockDbSet();
            context.Abouts.Add(Arg.Any<About>()).Returns(info => new About().ToMockEntityEntry());
            var service = GetAboutService(context);
            Action create = () => service.Create(default);
            create.Should().Throw<InvalidOperationException>().WithMessage("Inconsistent change count of 0.");
        }

        [Fact]
        public void CreateShouldThrowInvalidOperationExceptionWhenUnableToRetrieveAddedEntityDetails()
        {
            var context = Substitute.For<InformationContext>(Substitute.For<Configurer>());
            context.Abouts.Add(Arg.Any<About>()).Returns((EntityEntry<About>) null);
            var service = GetAboutService(context);
            Action create = () => service.Create(default);
            create.Should().Throw<InvalidOperationException>().WithMessage("Unable to retrieve added entry.");
        }

        [Fact]
        public void EditShouldEditAboutValue()
        {
            var context = Substitute.For<InformationContext>(Substitute.For<Configurer>()).Mock();
            var abouts = new[] { new About() }.ToMockDbSet();
            context.Abouts = abouts;
            context.SaveChanges().Returns(1);
            var service = GetAboutService(context);
            service.Edit("new value");
            // ReSharper disable once PossibleNullReferenceException -- value must be set for test to pass
            context.About.Value.Should().BeEquivalentTo("new value");
        }

        [Fact]
        public void EditShouldSaveNewValueToDataStore()
        {
            var context = Substitute.For<InformationContext>(Substitute.For<Configurer>()).Mock();
            var abouts = new[] { new About() }.ToMockDbSet();
            context.Abouts = abouts;
            context.SaveChanges().Returns(1);
            var service = GetAboutService(context);
            service.Edit(default);
            context.Received(1).SaveChanges();
        }

        [Fact]
        public void EditShouldThrowInvalidOperationExceptionWhenAboutWasNotFound()
        {
            var context = Substitute.For<InformationContext>(Substitute.For<Configurer>());
            var abouts = new About[0].ToMockDbSet();
            context.Abouts = abouts;
            var service = GetAboutService(context);
            Action edit = () => service.Edit(default);
            edit.Should().Throw<InvalidOperationException>().WithMessage("About not found.");
        }

        [Fact]
        public void EditShouldThrowInvalidOperationExceptionWhenUpdateResultsInInconsistentState()
        {
            var context = Substitute.For<InformationContext>(Substitute.For<Configurer>()).Mock();
            var abouts = new[] { new About() }.ToMockDbSet();
            context.Abouts = abouts;
            context.SaveChanges().Returns(0);
            var service = GetAboutService(context);
            Action edit = () => service.Edit(default);
            edit.Should().Throw<InvalidOperationException>().WithMessage("Inconsistent change count of 0.");
        }

        [Fact]
        public void GetAboutDetailsShouldReturnAboutDetails()
        {
            var context = Substitute.For<InformationContext>(Substitute.For<Configurer>());
            context.Abouts = new[] { new About { Value = "test" } }.ToMockDbSet();
            var service = GetAboutService(context);
            service.GetAboutDetails().Should().BeEquivalentTo(Option.Some("test"));
        }

        [Fact]
        public void GetAboutDetailsShouldReturnNoneWhenAboutIsNull()
        {
            var context = Substitute.For<InformationContext>(Substitute.For<Configurer>());
            context.Abouts = new List<About>().ToMockDbSet();
            var service = GetAboutService(context);
            service.GetAboutDetails().Should().BeEquivalentTo(Option.None<string>());
        }

        [Fact]
        public void GetAboutDetailsShouldReturnNoneWhenAboutValueIsNull()
        {
            var context = Substitute.For<InformationContext>(Substitute.For<Configurer>());
            context.Abouts = new[] { new About() }.ToMockDbSet();
            var service = GetAboutService(context);
            service.GetAboutDetails().Should().BeEquivalentTo(Option.None<string>());
        }
    }
}
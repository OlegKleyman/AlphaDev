﻿using System;
using System.Collections.Generic;
using System.Linq;
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
    public class ContactServiceTests
    {
        [NotNull]
        private ContactService GetContactService(InformationContext context)
        {
            var contextBuilder = Substitute.For<IContextFactory<InformationContext>>();
            contextBuilder.Create().Returns(context);
            return new ContactService(contextBuilder);
        }

        [Fact]
        public void ConstructorShouldInitializeContactService()
        {
            Action constructor = () => new ContactService(Substitute.For<IContextFactory<InformationContext>>());

            constructor.Should().NotThrow();
        }

        [Fact]
        public void GetContactDetailsShouldReturnContactDetails()
        {
            var context = Substitute.For<InformationContext>(Substitute.For<Configurer>());
            context.Contacts = new[] { new Contact { Value = "test" } }.ToMockDbSet();
            var service = GetContactService(context);
            service.GetDetails().Should().BeEquivalentTo(Option.Some("test"));
        }

        [Fact]
        public void GetContactDetailsShouldReturnNoneWhenContactIsNull()
        {
            var context = Substitute.For<InformationContext>(Substitute.For<Configurer>());
            context.Contacts = new List<Contact>().ToMockDbSet();
            var service = GetContactService(context);
            service.GetDetails().Should().BeEquivalentTo(Option.None<string>());
        }

        [Fact]
        public void GetContactDetailsShouldReturnNoneWhenContactValueIsNull()
        {
            var context = Substitute.For<InformationContext>(Substitute.For<Configurer>());
            context.Contacts = new[] { new Contact() }.ToMockDbSet();
            var service = GetContactService(context);
            service.GetDetails().Should().BeEquivalentTo(Option.None<string>());
        }

        [Fact]
        public void EditShouldEditContactValue()
        {
            var context = Substitute.For<InformationContext>(Substitute.For<Configurer>()).Mock();
            var contacts = new[] { new Contact() }.ToMockDbSet();
            context.Contacts = contacts;
            context.SaveChanges().Returns(1);
            var service = GetContactService(context);
            service.Edit("new value");
            // ReSharper disable once PossibleNullReferenceException -- value must be set for test to pass
            context.Contact.Value.Should().BeEquivalentTo("new value");
        }

        [Fact]
        public void EditShouldSaveNewValueToDataStore()
        {
            var context = Substitute.For<InformationContext>(Substitute.For<Configurer>()).Mock();
            var contacts = new[] { new Contact() }.ToMockDbSet();
            context.Contacts = contacts;
            context.SaveChanges().Returns(1);
            var service = GetContactService(context);
            service.Edit(default);
            context.Received(1).SaveChanges();
        }

        [Fact]
        public void EditShouldThrowInvalidOperationExceptionWhenContactWasNotFound()
        {
            var context = Substitute.For<InformationContext>(Substitute.For<Configurer>());
            var contacts = new Contact[0].ToMockDbSet();
            context.Contacts = contacts;
            var service = GetContactService(context);
            Action edit = () => service.Edit(default);
            edit.Should().Throw<InvalidOperationException>().WithMessage("Contact not found.");
        }

        [Fact]
        public void EditShouldThrowInvalidOperationExceptionWhenUpdateResultsInInconsistentState()
        {
            var context = Substitute.For<InformationContext>(Substitute.For<Configurer>()).Mock();
            var contacts = new[] { new Contact() }.ToMockDbSet();
            context.Contacts = contacts;
            context.SaveChanges().Returns(0);
            var service = GetContactService(context);
            Action edit = () => service.Edit(default);
            edit.Should().Throw<InvalidOperationException>().WithMessage("Inconsistent change count of 0.");
        }

        [Fact]
        public void CreateShouldSaveContactToDataStore()
        {
            var context = Substitute.For<InformationContext>(Substitute.For<Configurer>()).Mock();
            var contacts = new List<Contact>();
            context.Contacts = contacts.ToMockDbSet();
            context.SaveChanges().Returns(1);
            var service = GetContactService(context);
            service.Create("test");
            context.Contacts.Should().HaveCount(1).And.BeEquivalentTo(new { Value = "test" });
        }

        [Fact]
        public void CreateShouldSaveToDataStore()
        {
            var context = Substitute.For<InformationContext>(Substitute.For<Configurer>()).Mock();
            context.Contacts = new List<Contact>().ToMockDbSet();
            var entry = (EntityEntry<Contact>)FormatterServices.GetUninitializedObject(typeof(EntityEntry<Contact>));
            context.Contacts.Add(Arg.Any<Contact>()).Returns(entry);
            context.SaveChanges().Returns(1);
            var service = GetContactService(context);
            service.Create("test");
            context.Received(1).SaveChanges();
        }

        [Fact]
        public void CreateShouldThrowInvalidOperationExceptionWhenUnableSavingReturnsUnexpectedSaveCount()
        {
            var context = Substitute.For<InformationContext>(Substitute.For<Configurer>()).Mock();
            context.Contacts = new List<Contact>().ToMockDbSet();
            context.Contacts.Add(Arg.Any<Contact>()).Returns(new Contact().ToMockEntityEntry());
            var service = GetContactService(context);
            Action create = () => service.Create(default);
            create.Should().Throw<InvalidOperationException>().WithMessage("Inconsistent change count of 0.");
        }

        [Fact]
        public void CreateShouldThrowInvalidOperationExceptionWhenUnableToRetrieveAddedEntityDetails()
        {
            var context = Substitute.For<InformationContext>(Substitute.For<Configurer>());
            context.Contacts.Add(Arg.Any<Contact>()).Returns((EntityEntry<Contact>)null);
            var service = GetContactService(context);
            Action create = () => service.Create(default);
            create.Should().Throw<InvalidOperationException>().WithMessage("Unable to retrieve added entry.");
        }
    }
}

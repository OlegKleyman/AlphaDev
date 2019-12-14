using System;
using System.Collections.Generic;
using System.Linq;
using AlphaDev.Core.Data;
using AlphaDev.Core.Data.Contexts;
using AlphaDev.Core.Data.Entities;
using AlphaDev.Core.Data.Support;
using AlphaDev.Core.Tests.Unit.Extensions.Support;
using AlphaDev.Test.Core.Extensions;
using FluentAssertions;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using NSubstitute;
using Optional;
using Xunit;

namespace AlphaDev.Core.Tests.Unit
{
    public class ContactServiceTests
    {
        [NotNull]
        private ContactService GetContactService(DbSet<Contact> contacts) => new ContactService(contacts);

        [Fact]
        public void ConstructorShouldInitializeContactService()
        {
            Action constructor = () => new ContactService(Substitute.For<DbSet<Contact>>());

            constructor.Should().NotThrow();
        }

        [Fact]
        public void CreateShouldSaveContactToDataStore()
        {
            var contacts = new List<Contact>();
            var contactsDbSet = contacts.ToMockDbSet().WithAddReturns(contacts);
            var service = GetContactService(contactsDbSet);
            service.Create("test");
            contactsDbSet.Should().HaveCount(1).And.BeEquivalentTo(new { Value = "test" });
        }

        [Fact]
        public void EditShouldEditContactValue()
        {
            var contacts = new[] { new Contact() }.ToMockDbSet();
            var service = GetContactService(contacts);
            service.Edit("new value");
            contacts.Single().Value.Should().BeEquivalentTo("new value");
        }

        [Fact]
        public void EditShouldThrowInvalidOperationExceptionWhenContactWasNotFound()
        {
            var contacts = new Contact[0].ToMockDbSet();
            var service = GetContactService(contacts);
            Action edit = () => service.Edit(string.Empty);
            edit.Should().Throw<InvalidOperationException>().WithMessage("Contact not found.");
        }

        [Fact]
        public void GetContactDetailsShouldReturnContactDetails()
        {
            var contacts = new[] { new Contact { Value = "test" } }.ToMockDbSet();
            var service = GetContactService(contacts);
            service.GetContactDetails().Should().BeEquivalentTo(Option.Some("test"));
        }

        [Fact]
        public void GetContactDetailsShouldReturnNoneWhenContactIsNull()
        {
            var contacts = new List<Contact>().ToMockDbSet();
            var service = GetContactService(contacts);
            service.GetContactDetails().Should().BeEquivalentTo(Option.None<string>());
        }

        [Fact]
        public void GetContactDetailsShouldReturnNoneWhenContactValueIsNull()
        {
            var contacts = new[] { new Contact() }.ToMockDbSet();
            var service = GetContactService(contacts);
            service.GetContactDetails().Should().BeEquivalentTo(Option.None<string>());
        }
    }
}
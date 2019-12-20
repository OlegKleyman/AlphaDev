using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AlphaDev.Core.Data.Entities;
using AlphaDev.Test.Core.Extensions;
using FluentAssertions;
using FluentAssertions.Optional.Extensions;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using MockQueryable.NSubstitute;
using NSubstitute;
using Optional;
using Xunit;

namespace AlphaDev.Core.Tests.Unit
{
    public class ContactServiceTests
    {
        [NotNull]
        private ContactService GetContactService([NotNull] DbSet<Contact> contacts) => new ContactService(contacts);

        [Fact]
        public void ConstructorShouldInitializeContactService()
        {
            Action constructor = () => new ContactService(Substitute.For<DbSet<Contact>>()).EmptyCall();

            constructor.Should().NotThrow();
        }

        [Fact]
        public async Task CreateShouldSaveContactToDataStore()
        {
            var contactsDbSet = Enumerable.Empty<Contact>().AsQueryable().BuildMockDbSet();
            var service = GetContactService(contactsDbSet);
            await service.CreateAsync("test");
            await contactsDbSet.Received(1).AddAsync(Arg.Is<Contact>(contact => contact.Value == "test"));
        }

        [Fact]
        public async Task EditShouldEditContactValue()
        {
            var contacts = new[] { new Contact() }.AsQueryable().BuildMockDbSet();
            var service = GetContactService(contacts);
            await service.EditAsync("new value");
            contacts.Single().Value.Should().BeEquivalentTo("new value");
        }

        [Fact]
        public void EditShouldThrowInvalidOperationExceptionWhenContactWasNotFound()
        {
            var contacts = new Contact[0].AsQueryable().BuildMockDbSet();
            var service = GetContactService(contacts);
            Func<Task> edit = () => service.EditAsync(string.Empty);
            edit.Should().Throw<InvalidOperationException>().WithMessage("Contact not found.");
        }

        [Fact]
        public async Task GetContactDetailsAsyncShouldReturnContactDetails()
        {
            var contacts = new[] { new Contact { Value = "test" } }.AsQueryable().BuildMockDbSet();
            var service = GetContactService(contacts);
            (await service.GetContactDetailsAsync()).Should().HaveSome().Which.Should().Be("test");
        }

        [Fact]
        public async Task GetContactDetailsAsyncShouldReturnNoneWhenContactIsNull()
        {
            var contacts = new List<Contact>().AsQueryable().BuildMockDbSet();
            var service = GetContactService(contacts);
            (await service.GetContactDetailsAsync()).Should().BeNone();
        }
    }
}
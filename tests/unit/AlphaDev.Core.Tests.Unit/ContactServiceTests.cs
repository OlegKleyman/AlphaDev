using System;
using System.Linq;
using AlphaDev.Core.Data;
using AlphaDev.Core.Data.Contexts;
using AlphaDev.Core.Data.Entities;
using AlphaDev.Core.Data.Support;
using AlphaDev.Test.Core.Extensions;
using FluentAssertions;
using JetBrains.Annotations;
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
            context.Contacts = Enumerable.Empty<Contact>().ToMockDbSet();
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
            var context = Substitute.For<InformationContext>(Substitute.For<Configurer>());
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
            var context = Substitute.For<InformationContext>(Substitute.For<Configurer>());
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
            var context = Substitute.For<InformationContext>(Substitute.For<Configurer>());
            var contacts = new[] { new Contact() }.ToMockDbSet();
            context.Contacts = contacts;
            context.SaveChanges().Returns(0);
            var service = GetContactService(context);
            Action edit = () => service.Edit(default);
            edit.Should().Throw<InvalidOperationException>().WithMessage("Inconsistent change count.");
        }
    }
}

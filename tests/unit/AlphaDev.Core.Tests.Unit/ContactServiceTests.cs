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
        private ContactService GetInformationService(InformationContext context)
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
            var service = GetInformationService(context);
            service.GetDetails().Should().BeEquivalentTo(Option.Some("test"));
        }

        [Fact]
        public void GetContactDetailsShouldReturnNoneWhenContactIsNull()
        {
            var context = Substitute.For<InformationContext>(Substitute.For<Configurer>());
            context.Contacts = Enumerable.Empty<Contact>().ToMockDbSet();
            var service = GetInformationService(context);
            service.GetDetails().Should().BeEquivalentTo(Option.None<string>());
        }

        [Fact]
        public void GetContactDetailsShouldReturnNoneWhenContactValueIsNull()
        {
            var context = Substitute.For<InformationContext>(Substitute.For<Configurer>());
            context.Contacts = new[] { new Contact() }.ToMockDbSet();
            var service = GetInformationService(context);
            service.GetDetails().Should().BeEquivalentTo(Option.None<string>());
        }
    }
}

using System;
using AlphaDev.Core.Data.Contexts;
using AlphaDev.Core.Data.Entities;
using AlphaDev.Core.Data.Support;
using AlphaDev.Test.Core.Extensions;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace AlphaDev.Core.Data.Tests.Unit.Contexts
{
    public class InformationContextTests
    {
        private InformationContext GetInformationContext()
        {
            return Substitute.For<InformationContext>((Configurer) default);
        }

        [Fact]
        public void AboutShouldReturnAboutWhenExists()
        {
            var context = GetInformationContext();
            var abouts = new[] { new About { Id = true, Value = "value" } }.ToMockDbSet();
            context.Abouts = abouts;
            context.About.Should().BeEquivalentTo(new { Id = 1, Value = "value" });
        }

        [Fact]
        public void AboutShouldReturnNullWhenNoBlogsAreFound()
        {
            var context = GetInformationContext();
            var abouts = new About[0].ToMockDbSet();
            context.Abouts = abouts;
            context.About.Should().BeNull();
        }

        [Fact]
        public void AboutShouldThrowInvalidOperationExceptionWhenMultipleAboutsAreFound()
        {
            var context = GetInformationContext();
            var abouts = new[] { default(About), default }.ToMockDbSet();
            context.Abouts = abouts;
            Action about = () => context.About.EmptyCall();
            about.Should().Throw<InvalidOperationException>().WithMessage("Sequence contains more than one element");
        }

        [Fact]
        public void ContactShouldReturnContactWhenExists()
        {
            var context = GetInformationContext();
            var contacts = new[] { new Contact { Id = true, Value = "value" } }.ToMockDbSet();
            context.Contacts = contacts;
            context.Contact.Should().BeEquivalentTo(new { Id = 1, Value = "value" });
        }

        [Fact]
        public void ContactShouldReturnNullWhenNoBlogsAreFound()
        {
            var context = GetInformationContext();
            var contacts = new Contact[0].ToMockDbSet();
            context.Contacts = contacts;
            context.Contact.Should().BeNull();
        }

        [Fact]
        public void ContactShouldThrowInvalidOperationExceptionWhenMultipleContactsAreFound()
        {
            var context = GetInformationContext();
            var contacts = new[] { default(Contact), default }.ToMockDbSet();
            context.Contacts = contacts;
            Action contact = () => context.Contact.EmptyCall();
            contact.Should().Throw<InvalidOperationException>().WithMessage("Sequence contains more than one element");
        }
    }
}
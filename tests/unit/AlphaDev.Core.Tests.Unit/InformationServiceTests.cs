using System;
using AlphaDev.Core.Data.Contexts;
using AlphaDev.Core.Data.Entities;
using FluentAssertions;
using JetBrains.Annotations;
using Optional;
using Xunit;

namespace AlphaDev.Core.Tests.Unit
{
    public class InformationServiceTests
    {
        [NotNull]
        private static InformationService GetInformationService(InformationContext context)
        {
            return new InformationService(context);
        }

        [Fact]
        public void GetAboutDetailsShouldReturnAboutDetails()
        {
            var context = new MockInformationContext(nameof(GetAboutDetailsShouldReturnAboutDetails));
            context.Abouts.Add(new About
            {
                Value = "test"
            });
            context.SaveChanges();
            var service = GetInformationService(context);
            service.GetAboutDetails().Should().BeEquivalentTo(Option.Some("test"));
        }

        [Fact]
        public void GetAboutDetailsShouldReturnNoneWhenAboutDetailsAreNotFound()
        {
            var context =
                new MockInformationContext(nameof(GetAboutDetailsShouldReturnNoneWhenAboutDetailsAreNotFound));
            var service = GetInformationService(context);
            service.GetAboutDetails().Should().BeEquivalentTo(Option.None<string>());
        }

        [Fact]
        public void GetAboutDetailsShouldThrowInvalidOperationExceptionWhenMultipleAboutRecordsAreFound()
        {
            var context =
                new MockInformationContext(
                    nameof(GetAboutDetailsShouldThrowInvalidOperationExceptionWhenMultipleAboutRecordsAreFound));
            context.Abouts.AddRange(new About(), new About {Id = true});
            context.SaveChanges();
            var service = GetInformationService(context);
            Action getAboutDetails = () => service.GetAboutDetails();
            getAboutDetails.Should().Throw<InvalidOperationException>()
                .WithMessage("Multiple about information found.");
        }
    }
}
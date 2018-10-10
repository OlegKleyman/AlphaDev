using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using NSubstitute;

namespace AlphaDev.Core.Tests.Unit.Extensions.Support
{
    public static class DbContextMockExtensions
    {
        public static T Mock<T>([NotNull] this T context) where T : DbContext
        {
            context.Database.Returns(Substitute.For<DatabaseFacade>(context));

            return context;
        }
    }
}

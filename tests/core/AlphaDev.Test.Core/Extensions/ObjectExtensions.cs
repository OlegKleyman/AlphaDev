using AlphaDev.Test.Core.Support;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using NSubstitute;

namespace AlphaDev.Test.Core.Extensions
{
    public static class ObjectExtensions
    {
        public static void EmptyCall(this object target)
        {
        }

        [NotNull]
        public static EntityEntry<T> ToMockEntityEntry<T>([NotNull] this T target) where T : class
        {
            var stateManager = Substitute.For<StateManagerStub>();
            var entityType = new EntityType(typeof(T), new Model(), ConfigurationSource.Convention);
            var internalClrEntityEntry = new InternalClrEntityEntry(stateManager, entityType, target);

            var mockEntry = Substitute.For<EntityEntry<T>>(internalClrEntityEntry);
            mockEntry.Entity.Returns(target);
            return mockEntry;
        }
    }
}
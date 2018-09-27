using AlphaDev.Core.Data.Entities;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using NSubstitute;

namespace AlphaDev.Test.Core
{
    public static class ObjectExtensions
    {
        public static void EmptyCall(this object target)
        {
        }

        [NotNull]
        public static EntityEntry<T> ToMockEntityEntry<T>([NotNull] this T target) where T : class
        {
            var stateManager = Substitute.For<IStateManager>();
            var entityType = new EntityType(typeof(About), new Model(), ConfigurationSource.Convention);
            var internalClrEntityEntry = new InternalClrEntityEntry(stateManager, entityType, target);
            return new EntityEntry<T>(internalClrEntityEntry);
        }
    }
}
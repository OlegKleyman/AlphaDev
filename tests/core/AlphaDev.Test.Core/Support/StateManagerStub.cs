using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Update;

namespace AlphaDev.Test.Core.Support
{
    /// <summary>
    ///     Represents a stub class for <see cref="IStateManager" />. Only use with mocking frameworks.
    /// </summary>
    /// <remarks>
    ///     This stupid bug is back again in .net core 2.1 version of <see cref="IStateManager" />.
    ///     https://github.com/nsubstitute/NSubstitute/issues/378
    /// </remarks>
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public abstract class StateManagerStub : IStateManager
    {
        public virtual void ResetState()
        {
            throw new NotImplementedException();
        }

        public virtual InternalEntityEntry GetOrCreateEntry(object entity)
        {
            throw new NotImplementedException();
        }

        public virtual InternalEntityEntry GetOrCreateEntry(object entity, IEntityType entityType)
        {
            throw new NotImplementedException();
        }

        public virtual InternalEntityEntry CreateEntry(IDictionary<string, object> values, IEntityType entityType)
        {
            throw new NotImplementedException();
        }

        public virtual InternalEntityEntry StartTrackingFromQuery(IEntityType baseEntityType, object entity,
            in ValueBuffer valueBuffer,
            ISet<IForeignKey> handledForeignKeys)
        {
            throw new NotImplementedException();
        }

        public virtual void BeginTrackingQuery()
        {
            throw new NotImplementedException();
        }

        public virtual InternalEntityEntry TryGetEntry(IKey key, object[] keyValues)
        {
            throw new NotImplementedException();
        }

        public virtual InternalEntityEntry TryGetEntry(IKey key, in ValueBuffer valueBuffer, bool throwOnNullKey)
        {
            throw new NotImplementedException();
        }

        public virtual InternalEntityEntry TryGetEntry(object entity, bool throwOnNonUniqueness = true)
        {
            throw new NotImplementedException();
        }

        public virtual InternalEntityEntry TryGetEntry(object entity, IEntityType type)
        {
            throw new NotImplementedException();
        }

        public virtual InternalEntityEntry StartTracking(InternalEntityEntry entry)
        {
            throw new NotImplementedException();
        }

        public virtual void StopTracking(InternalEntityEntry entry)
        {
            throw new NotImplementedException();
        }

        public virtual void RecordReferencedUntrackedEntity(object referencedEntity, INavigation navigation,
            InternalEntityEntry referencedFromEntry)
        {
            throw new NotImplementedException();
        }

        public virtual IEnumerable<Tuple<INavigation, InternalEntityEntry>> GetRecordedReferers(object referencedEntity,
            bool clear)
        {
            throw new NotImplementedException();
        }

        public virtual InternalEntityEntry GetPrincipal(InternalEntityEntry dependentEntry, IForeignKey foreignKey)
        {
            throw new NotImplementedException();
        }

        public virtual InternalEntityEntry
            GetPrincipalUsingPreStoreGeneratedValues(InternalEntityEntry dependentEntry, IForeignKey foreignKey)
        {
            throw new NotImplementedException();
        }

        public virtual InternalEntityEntry GetPrincipalUsingRelationshipSnapshot(InternalEntityEntry dependentEntry,
            IForeignKey foreignKey)
        {
            throw new NotImplementedException();
        }

        public virtual void UpdateIdentityMap(InternalEntityEntry entry, IKey principalKey)
        {
            throw new NotImplementedException();
        }

        public virtual void UpdateDependentMap(InternalEntityEntry entry, IForeignKey foreignKey)
        {
            throw new NotImplementedException();
        }

        public virtual IEnumerable<InternalEntityEntry> GetDependentsFromNavigation(InternalEntityEntry principalEntry,
            IForeignKey foreignKey)
        {
            throw new NotImplementedException();
        }

        public virtual IEnumerable<InternalEntityEntry> GetDependents(InternalEntityEntry principalEntry,
            IForeignKey foreignKey)
        {
            throw new NotImplementedException();
        }

        public virtual IEnumerable<InternalEntityEntry> GetDependentsUsingRelationshipSnapshot(
            InternalEntityEntry principalEntry, IForeignKey foreignKey)
        {
            throw new NotImplementedException();
        }

        public virtual IReadOnlyList<IUpdateEntry> GetEntriesToSave()
        {
            throw new NotImplementedException();
        }

        public virtual int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            throw new NotImplementedException();
        }

        public virtual Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess,
            CancellationToken cancellationToken = new CancellationToken())
        {
            throw new NotImplementedException();
        }

        public virtual void AcceptAllChanges()
        {
            throw new NotImplementedException();
        }

        public virtual IEntityFinder CreateEntityFinder(IEntityType entityType)
        {
            throw new NotImplementedException();
        }

        public virtual TrackingQueryMode GetTrackingQueryMode(IEntityType entityType)
        {
            throw new NotImplementedException();
        }

        public virtual void EndSingleQueryMode()
        {
            throw new NotImplementedException();
        }

        public virtual void Unsubscribe()
        {
            throw new NotImplementedException();
        }

        public virtual void OnTracked(InternalEntityEntry internalEntityEntry, bool fromQuery)
        {
            throw new NotImplementedException();
        }

        public virtual void OnStateChanged(InternalEntityEntry internalEntityEntry, EntityState oldState)
        {
            throw new NotImplementedException();
        }

        public virtual IEnumerable<InternalEntityEntry> Entries { get; }
        public virtual int ChangedCount { get; set; }
        public virtual IInternalEntityEntryNotifier InternalEntityEntryNotifier { get; }
        public virtual IValueGenerationManager ValueGenerationManager { get; }
        public virtual DbContext Context { get; }
        public virtual IEntityMaterializerSource EntityMaterializerSource { get; }
        public virtual bool SensitiveLoggingEnabled { get; }
        public virtual IDiagnosticsLogger<DbLoggerCategory.Update> UpdateLogger { get; }

        public virtual event EventHandler<EntityTrackedEventArgs> Tracked
        {
            add => throw new NotImplementedException();
            remove => throw new NotImplementedException();
        }

        public virtual event EventHandler<EntityStateChangedEventArgs> StateChanged
        {
            add => throw new NotImplementedException();
            remove => throw new NotImplementedException();
        }
    }
}
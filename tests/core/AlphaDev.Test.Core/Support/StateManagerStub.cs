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
using Microsoft.EntityFrameworkCore.Query;
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

        public virtual Task ResetStateAsync(CancellationToken cancellationToken = new CancellationToken()) =>
            throw new NotImplementedException();

        public virtual InternalEntityEntry GetOrCreateEntry(object entity) => throw new NotImplementedException();

        public virtual InternalEntityEntry GetOrCreateEntry(object entity, IEntityType entityType) =>
            throw new NotImplementedException();

        public virtual InternalEntityEntry CreateEntry(IDictionary<string, object> values, IEntityType entityType) =>
            throw new NotImplementedException();

        public virtual InternalEntityEntry StartTrackingFromQuery(IEntityType baseEntityType, object entity,
            in ValueBuffer valueBuffer) => throw new NotImplementedException();

        public virtual InternalEntityEntry TryGetEntry(IKey key, object[] keyValues) =>
            throw new NotImplementedException();

        public virtual InternalEntityEntry TryGetEntry(IKey key, object[] keyValues, bool throwOnNullKey,
            out bool hasNullKey) => throw new NotImplementedException();

        public virtual InternalEntityEntry TryGetEntry(object entity, bool throwOnNonUniqueness = true) =>
            throw new NotImplementedException();

        public virtual InternalEntityEntry
            TryGetEntry(object entity, IEntityType type, bool throwOnTypeMismatch = true) =>
            throw new NotImplementedException();

        public virtual IEnumerable<InternalEntityEntry> GetEntriesForState(bool added = false, bool modified = false,
            bool deleted = false, bool unchanged = false) => throw new NotImplementedException();

        public virtual int GetCountForState(bool added = false, bool modified = false, bool deleted = false,
            bool unchanged = false) => throw new NotImplementedException();

        public virtual IEnumerable<TEntity> GetNonDeletedEntities<TEntity>() where TEntity : class =>
            throw new NotImplementedException();

        public virtual void StateChanging(InternalEntityEntry entry, EntityState newState)
        {
            throw new NotImplementedException();
        }

        public virtual InternalEntityEntry StartTracking(InternalEntityEntry entry) =>
            throw new NotImplementedException();

        public virtual void StopTracking(InternalEntityEntry entry, EntityState oldState)
        {
            throw new NotImplementedException();
        }

        public virtual void RecordReferencedUntrackedEntity(object referencedEntity, INavigation navigation,
            InternalEntityEntry referencedFromEntry)
        {
            throw new NotImplementedException();
        }

        public virtual IEnumerable<Tuple<INavigation, InternalEntityEntry>> GetRecordedReferrers(
            object referencedEntity, bool clear) => throw new NotImplementedException();

        public virtual InternalEntityEntry FindPrincipal(InternalEntityEntry dependentEntry, IForeignKey foreignKey) =>
            throw new NotImplementedException();

        public virtual InternalEntityEntry FindPrincipalUsingPreStoreGeneratedValues(InternalEntityEntry dependentEntry,
            IForeignKey foreignKey) => throw new NotImplementedException();

        public virtual InternalEntityEntry FindPrincipalUsingRelationshipSnapshot(InternalEntityEntry dependentEntry,
            IForeignKey foreignKey) => throw new NotImplementedException();

        public virtual void UpdateIdentityMap(InternalEntityEntry entry, IKey principalKey)
        {
            throw new NotImplementedException();
        }

        public virtual void UpdateDependentMap(InternalEntityEntry entry, IForeignKey foreignKey)
        {
            throw new NotImplementedException();
        }

        public virtual IEnumerable<InternalEntityEntry> GetDependentsFromNavigation(InternalEntityEntry principalEntry,
            IForeignKey foreignKey) => throw new NotImplementedException();

        public virtual IEnumerable<InternalEntityEntry> GetDependents(InternalEntityEntry principalEntry,
            IForeignKey foreignKey) => throw new NotImplementedException();

        public virtual IEnumerable<InternalEntityEntry> GetDependentsUsingRelationshipSnapshot(
            InternalEntityEntry principalEntry, IForeignKey foreignKey) => throw new NotImplementedException();

        public virtual IList<IUpdateEntry> GetEntriesToSave(bool cascadeChanges) => throw new NotImplementedException();

        public virtual int SaveChanges(bool acceptAllChangesOnSuccess) => throw new NotImplementedException();

        public virtual Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess,
            CancellationToken cancellationToken = new CancellationToken()) => throw new NotImplementedException();

        public virtual void AcceptAllChanges()
        {
            throw new NotImplementedException();
        }

        public virtual IEntityFinder CreateEntityFinder(IEntityType entityType) => throw new NotImplementedException();

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

        public virtual void CascadeChanges(bool force)
        {
            throw new NotImplementedException();
        }

        public virtual void CascadeDelete(InternalEntityEntry entry, bool force,
            IEnumerable<IForeignKey> foreignKeys = null)
        {
            throw new NotImplementedException();
        }

        public virtual StateManagerDependencies Dependencies { get; }

        public virtual CascadeTiming DeleteOrphansTiming { get; set; }

        public virtual CascadeTiming CascadeDeleteTiming { get; set; }

        public virtual IEnumerable<InternalEntityEntry> Entries { get; }

        public virtual int Count { get; }

        public virtual int ChangedCount { get; set; }

        public virtual IInternalEntityEntryNotifier InternalEntityEntryNotifier { get; }

        public virtual IValueGenerationManager ValueGenerationManager { get; }

        public virtual DbContext Context { get; }

        public virtual IModel Model { get; }

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
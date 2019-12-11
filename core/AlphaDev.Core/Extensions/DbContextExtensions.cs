using System;
using AlphaDev.Optional.Extensions;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Optional;

namespace AlphaDev.Core.Extensions
{
    public static class DbContextExtensions
    {
        [NotNull]
        public static EntityEntry<TEntity> AddAndSaveSingleOrThrow<TContext, TEntity>([NotNull] this TContext context,
            [NotNull] Func<TContext, DbSet<TEntity>> set, [NotNull] TEntity toAdd)
            where TContext : DbContext where TEntity : class
        {
            return set(context).Add(toAdd).SomeNotNull(() =>
                    new InvalidOperationException("Unable to retrieve added entry."))
                .Match(entry =>
                {
                    context.SaveSingleOrThrow();
                    return entry;
                }, exception => throw exception);
        }

        public static void UpdateAndSaveSingleOrThrow<TContext, TEntity>([NotNull] this TContext context,
            [NotNull] Func<TContext, TEntity?> getEntity, [NotNull] Action<TEntity> editEntity)
            where TContext : DbContext where TEntity : class
        {
            var option = getEntity(context).SomeWhenNotNull(GetEntityNotFoundException<TEntity>());
            option.MatchSome(editEntity);
            option.Match(entity => SaveSingleOrThrow(context), exception => throw exception);
        }

        [NotNull]
        private static Func<InvalidOperationException> GetEntityNotFoundException<TEntity>() where TEntity : class
        {
            return () => new InvalidOperationException($"{typeof(TEntity).Name} not found.");
        }

        public static void SaveSingleOrThrow([NotNull] this DbContext context)
        {
            if (context.Database is null)
            {
                throw new ArgumentException($"{nameof(context.Database)} is null.", nameof(context));
            }

            using var transaction = context.Database.BeginTransaction();
            context.SaveChanges().Some().Map(changes => changes.SomeWhen(i => i == 1,
                       () => new InvalidOperationException($"Inconsistent change count of {changes}.")))
                   .MatchSome(option => option.MatchNone(exception => throw exception));
            transaction.Commit();
        }

        [NotNull]
        public static EntityEntry<TEntity> DeleteSingleOrThrow<TEntity>([NotNull] this DbContext context,
            [NotNull] TEntity entity)
            where TEntity : class
        {
            return context.Remove(entity).SomeNotNull(GetEntityNotFoundException<TEntity>()).Match(entry =>
            {
                context.SaveSingleOrThrow();
                return entry;
            }, exception => throw exception);
        }
    }
}
using System;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Optional;

namespace AlphaDev.Core.Extensions
{
    public static class DbContextExtensions
    {
        public static void AddAndSaveSingleOrThrow<TContext, TEntity>([NotNull] this TContext context,
            [NotNull] Func<TContext, DbSet<TEntity>> set, [NotNull] TEntity toAdd)
            where TContext : DbContext where TEntity : class
        {
            set(context).Add(toAdd).SomeNotNull(() =>
                    new InvalidOperationException("Unable to retrieve added entry."))
                .Match(entry => context.SaveSingleOrThrow(), exception => throw exception);
        }

        public static void UpdateAndSaveSingleOrThrow<TContext, TEntity>([NotNull] this TContext context,
            [NotNull] Func<TContext, TEntity> getEntity, [NotNull] Action<TEntity> editEntity)
            where TContext : DbContext where TEntity : class
        {
            getEntity(context).SomeNotNull(() =>
                    new InvalidOperationException($"{typeof(TEntity).Name} not found."))
                .MapToAction(editEntity)
                .Match(entity => SaveSingleOrThrow(context), exception => throw exception);
        }

        public static void SaveSingleOrThrow([NotNull] this DbContext context)
        {
            using (var transaction = context.Database.BeginTransaction())
            {
                context.SaveChanges().Some().Map(changes => changes.SomeWhen(i => i == 1,
                        () => new InvalidOperationException($"Inconsistent change count of {changes}.")))
                    .MatchSome(option => option.MatchNone(exception => throw exception));
                transaction.Commit();
            }
        }
    }
}
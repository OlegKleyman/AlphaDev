using System;
using System.Collections.Generic;
using System.Linq;
using AlphaDev.Test.Core.Extensions;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using NSubstitute;

namespace AlphaDev.Core.Tests.Unit.Extensions.Support
{
    public static class DbContextMockExtensions
    {
        [NotNull]
        public static T Mock<T>([NotNull] this T context) where T : DbContext
        {
            context.Database.Returns(Substitute.For<DatabaseFacade>(context));
            return context;
        }

        [NotNull]
        public static T SuccessSingle<T>([NotNull] this T context) where T : DbContext
        {
            context.SaveChanges().Returns(1);
            return context;
        }

        [NotNull]
        public static TContext WithDbSet<TContext, TEntity>([NotNull] this TContext context,
            [NotNull] IEnumerable<TEntity> entities, [NotNull] Action<TContext, DbSet<TEntity>> setDbSet)
            where TContext : DbContext where TEntity : class
        {
            setDbSet(context, (entities as List<TEntity> ?? entities.ToList()).ToMockDbSet());
            return context;
        }

        [NotNull]
        public static TContext With<TContext, TEntity>([NotNull] this TContext context)
            where TContext : DbContext where TEntity : class
        {
            context.Remove(Arg.Any<TEntity>()).Returns(info => ((TEntity) info[0]).ToMockEntityEntry());
            return context;
        }
    }
}
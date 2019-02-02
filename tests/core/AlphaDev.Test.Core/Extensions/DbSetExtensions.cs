using System.Collections.Generic;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using NSubstitute;

namespace AlphaDev.Test.Core.Extensions
{
    public static class DbSetExtensions
    {
        [NotNull]
        public static DbSet<TEntity> WithAddReturns<TEntity>([NotNull] this DbSet<TEntity> set,
            ICollection<TEntity> collection)
            where TEntity : class
        {
            set.Add(Arg.Any<TEntity>()).Returns(info => info.Arg<TEntity>().ToMockEntityEntry())
                .AndDoes(info => collection.Add(info.Arg<TEntity>()));

            return set;
        }
    }
}
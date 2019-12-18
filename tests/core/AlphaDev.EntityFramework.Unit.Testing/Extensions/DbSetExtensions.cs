using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using NSubstitute;

namespace AlphaDev.EntityFramework.Unit.Testing.Extensions
{
    public static class DbSetExtensions
    {
        public static DbSet<TEntity> WithAddReturns<TEntity>(this DbSet<TEntity> set,
            ICollection<TEntity> collection)
            where TEntity : class
        {
            set.Add(Arg.Any<TEntity>())
               .Returns(info => info.Arg<TEntity>().ToMockEntityEntry())
               .AndDoes(info => collection.Add(info.Arg<TEntity>()));

            return set;
        }

        public static DbSet<TEntity> WithAddAsyncReturns<TEntity>(this DbSet<TEntity> set,
            ICollection<TEntity> collection)
            where TEntity : class
        {
            set.AddAsync(Arg.Any<TEntity>())
               .Returns(info => info.Arg<TEntity>().ToMockEntityEntry())
               .AndDoes(info => collection.Add(info.Arg<TEntity>()));

            return set;
        }
    }
}
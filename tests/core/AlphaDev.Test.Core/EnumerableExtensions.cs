using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using NSubstitute;

namespace AlphaDev.Test.Core
{
    public static class EnumerableExtensions
    {
        public static DbSet<T> ToMockDbSet<T>([NotNull] this IEnumerable<T> target) where T : class
        {
            var targetQuery = target.AsQueryable();
            var aboutSet = Substitute.For<DbSet<T>, IQueryable<T>>();
            var query = aboutSet.As<IQueryable<T>>();
            query.Provider.Returns(targetQuery.Provider);
            query.Expression.Returns(targetQuery.Expression);
            query.ElementType.Returns(targetQuery.ElementType);
            query.GetEnumerator().Returns(targetQuery.GetEnumerator());

            return aboutSet;
        }
    }
}
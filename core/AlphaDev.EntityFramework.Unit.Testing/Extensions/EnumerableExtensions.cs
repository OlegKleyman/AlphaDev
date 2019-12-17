using System.Collections.Generic;
using System.Linq;
using AlphaDev.EntityFramework.Unit.Testing.Support;
using Microsoft.EntityFrameworkCore;
using MockQueryable.NSubstitute;
using NSubstitute;

namespace AlphaDev.EntityFramework.Unit.Testing.Extensions
{
    public static class EnumerableExtensions
    {
        public static DbSet<T> ToMockDbSet<T>(this ICollection<T> target) where T : class =>
            target.AsQueryable().BuildMockDbSet();
    }
}
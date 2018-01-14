using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace AlphaDev.Test.Integration.Core.Extensions
{
    public static class DbContext
    {
        public static void DetachAll(this Microsoft.EntityFrameworkCore.DbContext context)
        {
            context.ChangeTracker.Entries().ToList()
                .ForEach(entry => context.Entry(entry.Entity).State = EntityState.Detached);
        }
    }
}

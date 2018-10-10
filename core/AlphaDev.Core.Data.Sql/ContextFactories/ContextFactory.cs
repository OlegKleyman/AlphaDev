using AlphaDev.Core.Data.Sql.Contexts;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace AlphaDev.Core.Data.Sql.ContextFactories
{
    public class ContextFactory<T> : IContextFactory<T> where T : DbContext
    {
        private readonly IDesignTimeDbContextFactory<T> _factory;

        public ContextFactory([NotNull] IDesignTimeDbContextFactory<T> factory)
        {
            _factory = factory;
        }

        public T Create()
        {
            return _factory.CreateDbContext(new string[0]);
        }
    }
}
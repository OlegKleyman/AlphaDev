using AlphaDev.Core.Data.Sql.Contexts;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Design;

namespace AlphaDev.Core.Data.Sql.ContextFactories
{
    public class InformationContextFactory : IContextFactory<InformationContext>
    {
        private readonly IDesignTimeDbContextFactory<InformationContext> _factory;

        public InformationContextFactory([NotNull] IDesignTimeDbContextFactory<InformationContext> factory)
        {
            _factory = factory;
        }

        [NotNull]
        public InformationContext Create()
        {
            return _factory.CreateDbContext(new string[0]);
        }
    }
}
using AlphaDev.Core.Data.Sql.Contexts;
using AlphaDev.Core.Data.Support;
using JetBrains.Annotations;

namespace AlphaDev.Core.Data.Sql
{
    public class InformationContextFactory : AlphaContextFactory<InformationContext>
    {
        [UsedImplicitly]
        public InformationContextFactory() : this(null)
        {
        }

        public InformationContextFactory(Configurer? configurer) : base(configurer)
        {
        }
    }
}
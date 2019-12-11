using AlphaDev.Core.Data.Sql.Contexts;
using AlphaDev.Core.Data.Support;
using JetBrains.Annotations;

namespace AlphaDev.Core.Data.Sql
{
    public class BlogContextFactory : AlphaContextFactory<BlogContext>
    {
        [UsedImplicitly]
        public BlogContextFactory() : this(null)
        {
        }

        public BlogContextFactory(Configurer? configurer) : base(configurer)
        {
        }
    }
}
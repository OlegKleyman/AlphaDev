using AlphaDev.Core.Data.Sql.Contexts;
using AlphaDev.Core.Data.Support;

namespace AlphaDev.Core.Data.Sql
{
    public class BlogContextFactory : AlphaContextFactory<BlogContext>
    {
        public BlogContextFactory() : this(null)
        {
        }

        public BlogContextFactory(Configurer configurer) : base(configurer)
        {
        }
    }
}
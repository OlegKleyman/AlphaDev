using AlphaDev.Core.Data.Sql.Contexts;
using AlphaDev.Core.Data.Support;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

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
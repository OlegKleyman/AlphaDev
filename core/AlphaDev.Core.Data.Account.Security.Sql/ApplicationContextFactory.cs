using AlphaDev.Core.Data.Account.Security.Sql.Contexts;
using AlphaDev.Core.Data.Contexts;
using AlphaDev.Core.Data.Sql;
using AlphaDev.Core.Data.Support;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace AlphaDev.Core.Data.Account.Security.Sql
{
    public class ApplicationContextFactory : AlphaContextFactory<ApplicationContext>
    {
        public ApplicationContextFactory() : this(null)
        {
        }

        public ApplicationContextFactory(Configurer configurer) : base(configurer)
        {
        }
    }
}
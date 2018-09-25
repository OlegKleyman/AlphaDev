using AlphaDev.Core;
using AlphaDev.Core.Data;
using AlphaDev.Core.Data.Account.Security.Sql;
using AlphaDev.Core.Data.Account.Security.Sql.Contexts;
using AlphaDev.Core.Data.Account.Security.Sql.Entities;
using AlphaDev.Core.Data.Sql;
using AlphaDev.Core.Data.Sql.Contexts;
using AlphaDev.Core.Data.Sql.Support;
using AlphaDev.Core.Data.Support;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.DependencyInjection;

namespace AlphaDev.Web.Bootstrap.Extensions
{
    public static class ServiceBuilderExtensions
    {
        public static IServiceCollection AddServices(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<IBlogService, BlogService>()
                .AddScoped<IAboutService, AboutService<InformationContext>>();
            return serviceCollection;
        }

        public static IServiceCollection AddContexts(this IServiceCollection serviceCollection,
            Configurer configurer)
        {
            serviceCollection.AddScoped<Core.Data.Contexts.BlogContext, BlogContext>(
                    provider => new BlogContextFactory(configurer).CreateDbContext())
                .AddScoped<Core.Data.Contexts.InformationContext, InformationContext>(
                    provider => new InformationContextFactory(configurer).CreateDbContext());
            return serviceCollection;
        }

        public static IServiceCollection AddIdentityContexts(this IServiceCollection serviceCollection,
            Configurer configurer)
        {
            serviceCollection.AddScoped<IdentityDbContext<User>, ApplicationContext>(provider =>
                new ApplicationContextFactory(configurer).CreateDbContext());

            return serviceCollection;
        }

        public static IServiceCollection AddDesignTimeFactories(this IServiceCollection serviceCollection,
            Configurer configurer)
        {
            serviceCollection
                .AddSingleton<IDesignTimeDbContextFactory<InformationContext>,
                    InformationContextFactory
                >(provider => new InformationContextFactory(configurer));

            return serviceCollection;
        }

        public static IServiceCollection AddContextFactories(this IServiceCollection serviceCollection)
        {
            return serviceCollection
                .AddSingleton<IContextFactory<InformationContext>,
                    Core.Data.Sql.ContextFactories.InformationContextFactory>();
        }
    }
}

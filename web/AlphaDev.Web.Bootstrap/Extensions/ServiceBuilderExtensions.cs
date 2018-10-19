using AlphaDev.Core;
using AlphaDev.Core.Data;
using AlphaDev.Core.Data.Account.Security.Sql;
using AlphaDev.Core.Data.Account.Security.Sql.Contexts;
using AlphaDev.Core.Data.Account.Security.Sql.Entities;
using AlphaDev.Core.Data.Contexts;
using AlphaDev.Core.Data.Sql;
using AlphaDev.Core.Data.Sql.ContextFactories;
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
                .AddScoped<IAboutService, AboutService>()
                .AddScoped<IContactService, ContactService>();
            return serviceCollection;
        }

        public static IServiceCollection AddContexts(this IServiceCollection serviceCollection,
            Configurer configurer)
        {
            serviceCollection.AddScoped<BlogContext, Core.Data.Sql.Contexts.BlogContext>(
                    provider => new BlogContextFactory(configurer).CreateDbContext())
                .AddScoped<InformationContext, Core.Data.Sql.Contexts.InformationContext>(
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
                .AddSingleton<IDesignTimeDbContextFactory<Core.Data.Sql.Contexts.InformationContext>,
                    InformationContextFactory
                >(provider => new InformationContextFactory(configurer))
                .AddSingleton<IDesignTimeDbContextFactory<Core.Data.Sql.Contexts.BlogContext>,
                    BlogContextFactory
                >(provider => new BlogContextFactory(configurer));

            return serviceCollection;
        }

        public static IServiceCollection AddContextFactories(this IServiceCollection serviceCollection)
        {
            return serviceCollection
                .AddSingleton<IContextFactory<InformationContext>,
                    ContextFactory<Core.Data.Sql.Contexts.InformationContext>>()
                .AddSingleton<IContextFactory<BlogContext>,
                    ContextFactory<Core.Data.Sql.Contexts.BlogContext>>();
        }
    }
}
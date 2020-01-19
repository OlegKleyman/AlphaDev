using System;
using System.Linq;
using System.Reflection;
using AlphaDev.BlogServices;
using AlphaDev.BlogServices.Core;
using AlphaDev.Collections.Extensions;
using AlphaDev.Core;
using AlphaDev.Core.Data.Account.Security.Sql;
using AlphaDev.Core.Data.Account.Security.Sql.Entities;
using AlphaDev.Core.Data.Contexts;
using AlphaDev.Core.Data.Sql;
using AlphaDev.Core.Data.Support;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.DependencyInjection;
using BlogService = AlphaDev.BlogServices.Web.BlogService;

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

        [NotNull]
        public static IServiceCollection AddContexts([NotNull] this IServiceCollection serviceCollection,
            Configurer configurer)
        {
            return serviceCollection
                   .AddScopedTo<BlogContext, DbContext>(
                       provider => new BlogContextFactory(configurer).CreateDbContext())
                   .AddScopedTo<InformationContext, DbContext>(provider =>
                       new InformationContextFactory(configurer).CreateDbContext())
                   .AddDbSets<BlogContext>()
                   .AddDbSets<InformationContext>();
        }

        [NotNull]
        public static IServiceCollection AddDbSets<T>([NotNull] this IServiceCollection serviceCollection)
            where T : DbContext
        {
            var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance)
                                      .Where(info =>
                                          info.PropertyType.IsGenericType &&
                                          info.PropertyType.GetGenericTypeDefinition() == typeof(DbSet<>));

            foreach (var property in properties)
            {
                serviceCollection.AddScoped(property.PropertyType,
                    provider => property.GetValue(provider.GetRequiredService<T>()));
            }

            return serviceCollection;
        }

        [NotNull]
        public static IServiceCollection AddScopedTo<TService1, TService2>(
            [NotNull] this IServiceCollection serviceCollection, Func<IServiceProvider, TService1> factory)
            where TService1 : class, TService2 where TService2 : class
        {
            return serviceCollection.AddScoped(factory)
                                    .AddScoped<TService2>(provider => provider.GetRequiredService<TService1>());
        }

        public static IServiceCollection AddDbContextSaveToken([NotNull] this IServiceCollection serviceCollection)
        {
            return serviceCollection.AddScoped(x =>
                x.GetServices<DbContext>()
                 .Select(context => (SaveAction) (() => context.SaveChangesAsync()))
                 .Combine());
        }

        [NotNull]
        public static IServiceCollection AddIdentityContexts([NotNull] this IServiceCollection serviceCollection,
            Configurer configurer)
        {
            serviceCollection.AddScopedTo<IdentityDbContext<User>, DbContext>(provider =>
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
    }
}
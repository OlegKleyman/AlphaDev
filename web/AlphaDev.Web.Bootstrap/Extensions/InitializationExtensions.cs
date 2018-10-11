using System;
using System.Threading.Tasks;
using AlphaDev.Core.Data.Account.Security.Sql.Entities;
using AlphaDev.Core.Data.Contexts;
using AlphaDev.Core.Data.Entities;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;

namespace AlphaDev.Web.Bootstrap.Extensions
{
    public static class InitializationExtensions
    {
        [NotNull]
        public static IApplicationBuilder UseDevelopmentBlogs([NotNull] this IApplicationBuilder builder)
        {
            using (var scope = builder.ApplicationServices.CreateScope())
            {
                if (scope.ServiceProvider.GetService<IHostingEnvironment>().IsDevelopment())
                {
                    var context = scope.ServiceProvider.GetService<BlogContext>();

                    context.Database.ExecuteSqlCommand("TRUNCATE TABLE Blogs");
                    context.Blogs.AddRange(
                        new Blog
                        {
                            Content = GetDevelopmentContent(),
                            Title = "testing",
                            Created = new DateTime(2016, 7, 7),
                            Modified = new DateTime(2017, 7, 10)
                        }, new Blog
                        {
                            Content = GetDevelopmentContent(),
                            Title = "testing two",
                            Created = new DateTime(2016, 7, 7),
                            Modified = new DateTime(2017, 7, 10)
                        });

                    context.SaveChanges();
                }
            }

            return builder;
        }

        [NotNull]
        public static IApplicationBuilder UseDevelopmentInformation([NotNull] this IApplicationBuilder builder)
        {
            using (var scope = builder.ApplicationServices.CreateScope())
            {
                if (scope.ServiceProvider.GetService<IHostingEnvironment>().IsDevelopment())
                {
                    var context = scope.ServiceProvider.GetService<InformationContext>();

                    context.Abouts.AddRange(
                        new About
                        {
                            Value = GetDevelopmentContent()
                        });

                    context.Contacts.AddRange(
                        new Contact
                        {
                            Value = GetDevelopmentContent()
                        });

                    context.SaveChanges();
                }
            }

            return builder;
        }

        [NotNull]
        public static IApplicationBuilder UseAllDatabaseMigrations([NotNull] this IApplicationBuilder builder)
        {
            using (var scope = builder.ApplicationServices.CreateScope())
            {
                if (scope.ServiceProvider.GetService<IHostingEnvironment>().IsDevelopment())
                {
                    scope.ServiceProvider.GetService<BlogContext>().Database.EnsureDeleted();
                    scope.ServiceProvider.GetService<InformationContext>().Database.EnsureDeleted();
                    scope.ServiceProvider.GetService<IdentityDbContext<User>>().Database.EnsureDeleted();
                }

                scope.ServiceProvider.GetService<BlogContext>().Database.Migrate();
                scope.ServiceProvider.GetService<InformationContext>().Database.Migrate();
                scope.ServiceProvider.GetService<IdentityDbContext<User>>().Database.Migrate();
            }

            return builder;
        }

        [NotNull]
        public static IApplicationBuilder UseLoggerSettings([NotNull] this IApplicationBuilder builder)
        {
            using (var scope = builder.ApplicationServices.CreateScope())
            {
                var configuration = scope.ServiceProvider.GetService<IConfiguration>();
                var loggerFactory = scope.ServiceProvider.GetService<ILoggerFactory>();

                loggerFactory.AddConsole(configuration.GetSection("Logging"));
                loggerFactory.AddDebug();
                var logger = new LoggerConfiguration().ReadFrom.Configuration(configuration).CreateLogger();
                loggerFactory.AddSerilog(logger);
            }

            return builder;
        }

        [NotNull]
        public static IApplicationBuilder UseExceptionHandling([NotNull] this IApplicationBuilder builder)
        {
            using (var scope = builder.ApplicationServices.CreateScope())
            {
                if (scope.ServiceProvider.GetService<IHostingEnvironment>().IsDevelopment())
                {
                    builder.UseDeveloperExceptionPage();
                }
                else builder.UseExceptionHandler("/Default/Error/500");
            }

            return builder;
        }

        [ItemNotNull]
        public static async Task<IApplicationBuilder> UseDevelopmentUser([NotNull] this IApplicationBuilder builder)
        {
            using (var scope = builder.ApplicationServices.CreateScope())
            {
                if (scope.ServiceProvider.GetService<IHostingEnvironment>().IsDevelopment())
                {
                    var userManager = scope.ServiceProvider.GetService<UserManager<User>>();

                    await userManager.CreateAsync(new User
                    {
                        UserName = "something@something.com",
                        Email = "something@something.com"
                    }, "H3ll04321!");
                }
            }

            return builder;
        }

        [NotNull]
        private static string GetDevelopmentContent()
        {
            return "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Duis porttitor rutrum "
                   + "\n\n```csharp\nusing Foo.Bar;\n\nnamespace Baz\n{\n    [Qux(\"...\")]\n    public class Quux : "
                   + "ICorge\n    {\n        public static Grault<Garply> Waldo(Fred plugh)\n        {\n            "
                   + "Console.WriteLine($\"Hello {plugh.ToString()}!\");\n            return new Grault<Garply>("
                   + "new int[] { 1, 2, 3 })\n        }\n    }\n}\n```\n"
                   + "tellus, a fermentum lacus congue eget. Interdum et malesuada fames ac ante ipsum primis "
                   + "in faucibus. Proin imperdiet nisl ullamcorper nisi accumsan, in ultrices diam iaculis. "
                   + "Interdum et malesuada fames ac ante ipsum "
                   + "primis in faucibus. Vestibulum sed pulvinar lorem. Maecenas eget sollicitudin odio. Sed "
                   + "mattis sem sit amet orci pharetra, at laoreet felis faucibus. Donec eleifend urna et "
                   + "sodales laoreet. Duis dolor sem, scelerisque sit amet ex ut, placerat imperdiet est. "
                   + "Nunc at sodales ex, sed scelerisque lacus. Nam nec efficitur libero. Morbi bibendum "
                   + "orci ipsum, in hendrerit ex vestibulum blandit. Vivamus vitae magna ultrices, "
                   + "lobortis nulla ac, pulvinar elit. Vivamus ullamcorper feugiat erat, in efficitur lacus "
                   + "euismod vitae.<br />Lorem ipsum dolor sit amet, consectetur adipiscing elit. Duis porttitor rutrum "
                   + "tellus, a fermentum lacus congue eget. Interdum et malesuada fames ac ante ipsum primis "
                   + "in faucibus. Proin imperdiet nisl ullamcorper nisi accumsan, in ultrices diam iaculis. "
                   + "Vestibulum luctus consectetur vestibulum. Interdum et malesuada fames ac ante ipsum "
                   + "primis in faucibus. Vestibulum sed pulvinar lorem. Maecenas eget sollicitudin odio. Sed "
                   + "mattis sem sit amet orci pharetra, at laoreet felis faucibus. Donec eleifend urna et "
                   + "sodales laoreet. Duis dolor sem, scelerisque sit amet ex ut, placerat imperdiet est. "
                   + "Nunc at sodales ex, sed scelerisque lacus. Nam nec efficitur libero. Morbi bibendum "
                   + "orci ipsum, in hendrerit ex vestibulum blandit. Vivamus vitae magna ultrices, "
                   + "lobortis nulla ac, pulvinar elit. Vivamus ullamcorper feugiat erat, in efficitur lacus "
                   + "euismod vitae.";
        }
    }
}
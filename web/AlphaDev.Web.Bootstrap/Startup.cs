using System;
using AlphaDev.Core;
using AlphaDev.Core.Data.Contexts;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using Blog = AlphaDev.Core.Data.Entities.Blog;

namespace AlphaDev.Web.Bootstrap
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            var config = services.BuildServiceProvider().GetService<IConfiguration>();
            services.AddScoped<IBlogService, BlogService>();
            services.AddScoped<BlogContext, Core.Data.Sql.Contexts.BlogContext>(
                provider => new Core.Data.Sql.Contexts.BlogContext(config.GetConnectionString("default")));
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory,
            IApplicationLifetime lifetime, IServiceProvider provider, IConfiguration configuration)
        {
            loggerFactory.AddConsole(configuration.GetSection("Logging"));
            loggerFactory.AddDebug();
            var logger = new LoggerConfiguration().ReadFrom.Configuration(configuration).CreateLogger();
            loggerFactory.AddSerilog(logger);

            using (var blogContext = provider.GetService<BlogContext>())
            {
                if (env.IsDevelopment())
                {
                    blogContext.Database.EnsureDeleted();

                    app.UseDeveloperExceptionPage();
                }
                else
                {
                    app.UseExceptionHandler("/Default/Error/500");
                }

                app.UseStatusCodePagesWithReExecute("/default/error/{0}");

                blogContext.Database.Migrate();

                if (env.IsDevelopment())
                {
                    blogContext.Blogs.Add(
                        new Blog
                        {
                            Content = GetDevelopmentContent(),
                            Title = "testing",
                            Created = new DateTime(2016, 7, 7),
                            Modified = new DateTime(2017, 7, 10)
                        });

                    blogContext.Blogs.Add(
                        new Blog
                        {
                            Content = GetDevelopmentContent(),
                            Title = "testing two",
                            Created = new DateTime(2016, 7, 7),
                            Modified = new DateTime(2017, 7, 10)
                        });
                    blogContext.SaveChanges();
                }
            }

            app.UseStaticFiles();

            app.UseMvc(routes => { routes.MapRoute("default", "{controller=Default}/{action=Index}/{id?}"); });
        }

        private string GetDevelopmentContent()
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
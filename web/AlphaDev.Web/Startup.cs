namespace AlphaDev.Web
{
    using System;

    using AlphaDev.Core.Data.Entities;
    using AlphaDev.Core.Data.Sql;
    using AlphaDev.Core.Data.Sql.Contexts;

    using AppDev.Core;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Hosting.Builder;
    using Microsoft.AspNetCore.Http.Features;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
        
    public class Startup
    {
        public Startup(IHostingEnvironment env, IConfigurationBuilder builder)
        {
            builder
                .AddJsonFile("appsettings.json", true, true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", true).AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<IBlogService, BlogService>();
            services.AddSingleton<Core.Data.Contexts.BlogContext, BlogContext>(
                provider => new BlogContext(Configuration.GetConnectionString("default")));

            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, IApplicationLifetime lifetime, IServiceProvider provider)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                var blogContext = provider.GetService<Core.Data.Contexts.BlogContext>();
                blogContext.Database.EnsureDeleted();
                blogContext.Database.Migrate();
                blogContext.Blogs.Add(
                    new Core.Data.Entities.Blog
                        {
                            Content = GetDevelopmentContent(),
                            Title = "testing",
                            Created = new DateTime(2016, 7, 7),
                            Modified = new DateTime(2017, 7, 10)
                        });
                blogContext.SaveChanges();
                blogContext.Database.CloseConnection();

                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseMvc(routes => { routes.MapRoute("default", "{controller=Default}/{action=Index}/{id?}"); });
        }

        private string GetDevelopmentContent() =>
            "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Duis porttitor rutrum "
            + "tellus, a fermentum lacus congue eget. Interdum et malesuada fames ac ante ipsum primis "
            + "in faucibus. Proin imperdiet nisl ullamcorper nisi accumsan, in ultrices diam iaculis. "
            + "Vestibulum luctus consectetur vestibulum. Interdum et malesuada fames ac ante ipsum "
            + "primis in faucibus. Vestibulum sed pulvinar lorem. Maecenas eget sollicitudin odio. Sed "
            + "mattis sem sit amet orci pharetra, at laoreet felis faucibus. Donec eleifend urna et "
            + "sodales laoreet. Duis dolor sem, scelerisque sit amet ex ut, placerat imperdiet est. "
            + "Nunc at sodales ex, sed scelerisque lacus. Nam nec efficitur libero. Morbi bibendum "
            + "orci ipsum, in hendrerit ex vestibulum blandit. Vivamus vitae magna ultrices, "
            + "lobortis nulla ac, pulvinar elit. Vivamus ullamcorper feugiat erat, in efficitur lacus "
            + "euismod vitae." + "<br />Lorem ipsum dolor sit amet, consectetur adipiscing elit. Duis porttitor rutrum "
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
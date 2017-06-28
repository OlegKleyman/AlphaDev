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
                .AddJsonFile("connectionstrings.json", true, true)
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
                blogContext.Blogs.Add(new Core.Data.Entities.Blog { Content = "testing the blog", Title = "testing" });
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
    }
}
using AlphaDev.Core;
using AlphaDev.Core.Data.Account.Security.Sql.Contexts;
using AlphaDev.Core.Data.Account.Security.Sql.Entities;
using AlphaDev.Core.Data.Contexts;
using AlphaDev.Web.Bootstrap.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AlphaDev.Web.Bootstrap
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            var config = services.BuildServiceProvider().GetService<IConfiguration>();

            services.AddScoped<IdentityDbContext<User>, ApplicationContext>(provider =>
                new ApplicationContext(config.GetConnectionString("defaultSecurity")));
            services.AddScoped<IBlogService, BlogService>();
            services.AddScoped<BlogContext, Core.Data.Sql.Contexts.BlogContext>(
                provider => new Core.Data.Sql.Contexts.BlogContext(config.GetConnectionString("default")));
            services.AddIdentity<User, IdentityRole>().AddDefaultTokenProviders()
                .AddEntityFrameworkStores<IdentityDbContext<User>>();

            services.ConfigureApplicationCookie(options => { options.LoginPath = "/account/login"; });

            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseExceptionHandling()
                .UseLoggerSettings()
                .UseStatusCodePagesWithReExecute("/default/error/{0}")
                .UseAllDatabaseMigrations()
                .UseDevelopmentBlogs()
                .UseDevelopmentUser().GetAwaiter().GetResult()
                .UseStaticFiles()
                .UseAuthentication()
                .UseMvc(routes => routes.MapRoute("default", "{controller=Default}/{action=Index}/{id?}"));
        }
    }
}
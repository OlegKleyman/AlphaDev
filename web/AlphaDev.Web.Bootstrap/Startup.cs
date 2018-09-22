using AlphaDev.Core;
using AlphaDev.Core.Data;
using AlphaDev.Core.Data.Account.Security.Sql;
using AlphaDev.Core.Data.Account.Security.Sql.Contexts;
using AlphaDev.Core.Data.Account.Security.Sql.Entities;
using AlphaDev.Core.Data.Contexts;
using AlphaDev.Core.Data.Sql;
using AlphaDev.Core.Data.Sql.ContextFactories;
using AlphaDev.Core.Data.Sql.Support;
using AlphaDev.Web.Bootstrap.Extensions;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using InformationContextFactory = AlphaDev.Core.Data.Sql.ContextFactories.InformationContextFactory;

namespace AlphaDev.Web.Bootstrap
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        [UsedImplicitly]
        public void ConfigureServices(IServiceCollection services)
        {
            var config = services.BuildServiceProvider().GetService<IConfiguration>();
            var securitySqlConfigurer = new SqlConfigurer(config.GetConnectionString("defaultSecurity"));
            var defaultSqlConfigurer = new SqlConfigurer(config.GetConnectionString("default"));
            services.AddSingleton<IPrefixGenerator, PrefixGenerator>()
                .AddServices()
                .AddContexts(defaultSqlConfigurer)
                .AddIdentityContexts(securitySqlConfigurer)
                .AddContextFactories()
                .AddSingleton<IDateProvider, DateProvider>()
                .AddDesignTimeFactories(defaultSqlConfigurer)
                .AddIdentity<User, IdentityRole>()
                .AddDefaultTokenProviders()
                .AddEntityFrameworkStores<IdentityDbContext<User>>().Services
                .ConfigureApplicationCookie(options => { options.LoginPath = "/account/login"; })
                .AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        [UsedImplicitly]
        public void Configure([NotNull] IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseExceptionHandling()
                .UseLoggerSettings()
                .UseStatusCodePagesWithReExecute("/default/error/{0}")
                .UseAllDatabaseMigrations()
                .UseDevelopmentBlogs()
                .UseDevelopmentInformation()
                .UseDevelopmentUser().GetAwaiter().GetResult()
                .UseStaticFiles()
                .UseAuthentication()
                .UseMvc(routes => routes.MapRoute("default", "{controller=Default}/{action=Index}/{id?}"));
        }
    }
}
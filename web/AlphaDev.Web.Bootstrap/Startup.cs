using AlphaDev.Core;
using AlphaDev.Core.Data.Account.Security.Sql;
using AlphaDev.Core.Data.Account.Security.Sql.Contexts;
using AlphaDev.Core.Data.Account.Security.Sql.Entities;
using AlphaDev.Core.Data.Contexts;
using AlphaDev.Core.Data.Sql;
using AlphaDev.Core.Data.Sql.Support;
using AlphaDev.Web.Bootstrap.Extensions;
using JetBrains.Annotations;
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
        [UsedImplicitly]
        public void ConfigureServices(IServiceCollection services)
        {
            var config = services.BuildServiceProvider().GetService<IConfiguration>();
            var securitySqlConfigurer = new SqlConfigurer(config.GetConnectionString("defaultSecurity"));
            var defaultSqlConfigurer = new SqlConfigurer(config.GetConnectionString("default"));
            services.AddSingleton<IPrefixGenerator, PrefixGenerator>()
                .AddScoped<IdentityDbContext<User>, ApplicationContext>(provider =>
                    new ApplicationContextFactory(securitySqlConfigurer).CreateDbContext())
                .AddScoped<IBlogService, BlogService>()
                .AddScoped<IInformationService, InformationService>()
                .AddSingleton<IDateProvider, DateProvider>()
                .AddScoped<BlogContext, Core.Data.Sql.Contexts.BlogContext>(
                    provider => new BlogContextFactory(defaultSqlConfigurer).CreateDbContext())
                .AddScoped<InformationContext, Core.Data.Sql.Contexts.InformationContext>(
                    provider => new InformationContextFactory(defaultSqlConfigurer).CreateDbContext())
                .AddIdentity<User, IdentityRole>().AddDefaultTokenProviders()
                .AddEntityFrameworkStores<IdentityDbContext<User>>();

            services.ConfigureApplicationCookie(options => { options.LoginPath = "/account/login"; }).AddMvc();
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
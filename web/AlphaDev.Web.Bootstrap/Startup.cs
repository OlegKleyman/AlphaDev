using AlphaDev.Core;
using AlphaDev.Core.Data.Account.Security.Sql.Entities;
using AlphaDev.Core.Data.Sql.Support;
using AlphaDev.Web.Bootstrap.Extensions;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;

namespace AlphaDev.Web.Bootstrap
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        [UsedImplicitly]
        public void ConfigureServices(IServiceCollection services)
        {
            var securitySqlConfigurer = new Sql2008Configurer(_configuration.GetConnectionString("defaultSecurity"));
            var defaultSqlConfigurer = new Sql2008Configurer(_configuration.GetConnectionString("default"));
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
                .AddLogging(builder =>
                {
                    builder.AddConfiguration(_configuration.GetSection("Logging"));
                    builder.AddConsole();
                    builder.AddDebug();
                    var logger = new LoggerConfiguration().ReadFrom.Configuration(_configuration).CreateLogger();
                    builder.AddSerilog(logger);
                })
                .AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        [UsedImplicitly]
        public void Configure([NotNull] IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseExceptionHandling()
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
using System;
using AlphaDev.Core;
using AlphaDev.Core.Data.Account.Security.Sql.Entities;
using AlphaDev.Core.Data.Sql.Support;
using AlphaDev.Paging;
using AlphaDev.Web.Bootstrap.Extensions;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Refit;
using Serilog;

namespace AlphaDev.Web.Bootstrap
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration) => _configuration = configuration;

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        [UsedImplicitly]
        public void ConfigureServices(IServiceCollection services)
        {
            var securitySqlConfigurer = new SqlConfigurer(_configuration.GetConnectionString("defaultSecurity"));
            var defaultSqlConfigurer = new SqlConfigurer(_configuration.GetConnectionString("default"));
            var blogServiceAddress = _configuration.GetConnectionString("blogService");
            services.AddRefitClient<Services.Web.IBlogService>()
                    .ConfigureHttpClient(client => client.BaseAddress = new Uri(blogServiceAddress))
                    .Services.AddSingleton<IPrefixGenerator, PrefixGenerator>()
                    .AddServices()
                    .AddContexts(defaultSqlConfigurer)
                    .AddDbContextSaveToken()
                    .AddScoped<ISaveToken, SaveToken>()
                    .AddIdentityContexts(securitySqlConfigurer)
                    .AddSingleton<IDateProvider, DateProvider>()
                    .AddDesignTimeFactories(defaultSqlConfigurer)
                    .AddIdentity<User, IdentityRole>()
                    .AddDefaultTokenProviders()
                    .AddEntityFrameworkStores<IdentityDbContext<User>>()
                    .Services
                    .ConfigureApplicationCookie(options => { options.LoginPath = "/account/login"; })
                    .AddLogging(builder =>
                    {
                        builder.AddConfiguration(_configuration.GetSection("Logging"));
                        builder.AddConsole();
                        builder.AddDebug();
                        var logger = new LoggerConfiguration()
                                     .ReadFrom.Configuration(_configuration)
                                     .CreateLogger();
                        builder.AddSerilog(logger);
                    })
                    .AddMvc(options => { options.EnableEndpointRouting = false; })
                    .SetCompatibilityVersion(CompatibilityVersion.Version_3_0)
                    .Services
                    .AddSingleton(provider => new PagesSettings(9, 9, 10));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        [UsedImplicitly]
        public void Configure([NotNull] IApplicationBuilder app)
        {
            app.UseExceptionHandling()
               .UseStatusCodePagesWithReExecute("/default/error/{0}")
               .UseAllDatabaseMigrations()
               .UseDevelopmentBlogs()
               .UseDevelopmentInformation()
               .UseDevelopmentUser()
               .GetAwaiter()
               .GetResult()
               .UseStaticFiles()
               .UseAuthentication()
               .UseMvc(routes => routes.MapRoute("default", "{controller=Default}/{action=Index}/{id?}"));
        }
    }
}
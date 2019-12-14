using System;
using System.Linq;
using System.Threading.Tasks;
using AlphaDev.Collections.Extensions;
using AlphaDev.Core;
using AlphaDev.Core.Data.Account.Security.Sql.Entities;
using AlphaDev.Core.Data.Contexts;
using AlphaDev.Core.Data.Sql.Support;
using AlphaDev.Web.Bootstrap.Extensions;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
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
            _ = services.AddSingleton<IPrefixGenerator, PrefixGenerator>()
                        .AddServices()
                        .AddContexts(defaultSqlConfigurer)
                        .AddDbSets()
                        .AddScoped<DbContext>(provider => provider.GetRequiredService<InformationContext>())
                        .AddScoped<DbContext>(provider => provider.GetRequiredService<BlogContext>())
                        // ReSharper disable once CoVariantArrayConversion - Can combine delegates of the same type
                        .AddScoped(x => x.GetServices<DbContext>().Select(context => (SaveAction)(() => context.SaveChangesAsync())).Combine())
                        .AddScoped<ISaveToken, SaveToken>()
                        .AddIdentityContexts(securitySqlConfigurer)
                        .AddContextFactories()
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
                        .AddMvc(options =>
                        {
                            options.EnableEndpointRouting = false;
                        })
                        .SetCompatibilityVersion(CompatibilityVersion.Version_3_0);
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
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using AlphaDev.BlogServices;
using AlphaDev.BlogServices.Core;
using AlphaDev.Core;
using AlphaDev.Core.Data.Account.Security.Sql;
using AlphaDev.Core.Data.Account.Security.Sql.Entities;
using AlphaDev.Core.Data.Contexts;
using AlphaDev.Core.Data.Sql.Support;
using AlphaDev.Core.Extensions;
using AlphaDev.Security;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;

namespace AlphaDev.Web.Api
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration) => _configuration = configuration;

        public void ConfigureServices(IServiceCollection services)
        {
            var blogConfigurer = new SqlConfigurer(_configuration.GetConnectionString("default"));
            var securityConfigurer = new SqlConfigurer(_configuration.GetConnectionString("security"));

            services.AddScoped<BlogContext>(provider => new Core.Data.Sql.Contexts.BlogContext(blogConfigurer));
            services.AddScoped<IdentityDbContext<User>>(provider =>
                new ApplicationContextFactory(securityConfigurer).CreateDbContext());
            services.AddIdentityCore<User>().AddEntityFrameworkStores<IdentityDbContext<User>>();
            services.AddScoped(provider => provider.GetRequiredService<BlogContext>().Blogs);
            services.AddSingleton<IDateProvider, DateProvider>();
            services.AddScoped<IBlogService, BlogService>();
            services.AddCors(options => options.AddPolicy("CorsPolicy",
                builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader().AllowCredentials().Build()));
            services.AddControllers();
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(options => options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = _configuration["Jwt:Issuer"],
                        ValidAudience = _configuration["Jwt:Audience"],
                        IssuerSigningKey =
                            new SymmetricSecurityKey(Encoding.Default.GetBytes(_configuration["Jwt:Key"]))
                    });
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_3_0);
            services.AddSingleton<SecurityTokenHandler, JwtSecurityTokenHandler>();
            services.AddSingleton<IJwtTokenBuilderFactory, JwtTokenBuilderFactory>();
            services.AddSingleton(provider =>
                _configuration.GetSection("Jwt")
                              .Get<TokenSettings>()
                              .To(settings =>
                                  provider.GetRequiredService<IJwtTokenBuilderFactory>().Create(settings)));
            services.AddSingleton<ISecurityTokenWriter, JwtSecurityTokenWriter>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
            }
            app.UseDeveloperExceptionPage();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints => endpoints.MapControllers());
        }
    }
}
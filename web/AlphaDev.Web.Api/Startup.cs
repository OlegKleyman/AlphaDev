using System;
using AlphaDev.BlogServices;
using AlphaDev.BlogServices.Core;
using AlphaDev.Core;
using AlphaDev.Core.Data.Contexts;
using AlphaDev.Core.Data.Sql.Support;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Blog = AlphaDev.Core.Data.Entities.Blog;

namespace AlphaDev.Web.Api
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration) => _configuration = configuration;

        public void ConfigureServices(IServiceCollection services)
        {
            var configurer = new SqlConfigurer(_configuration.GetConnectionString("default"));
            services.AddScoped<BlogContext>(provider => new Core.Data.Sql.Contexts.BlogContext(configurer));
            services.AddScoped(provider => provider.GetRequiredService<BlogContext>().Blogs);
            services.AddSingleton<IDateProvider, DateProvider>();
            services.AddScoped<IBlogService, BlogService>();
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints => endpoints.MapControllers());
        }
    }
}
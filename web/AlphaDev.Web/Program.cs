using System.IO;
using System.Reflection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AlphaDev.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var contentRoot = Directory.GetCurrentDirectory();

            var host = new WebHostBuilder().UseKestrel().UseContentRoot(contentRoot).UseIISIntegration()
                .UseStartup<AlphaDev.Web.Bootstrap.Startup>().UseApplicationInsights().ConfigureServices(
                    services => services.AddSingleton<IConfigurationBuilder, IConfigurationBuilder>(
                        provider => new ConfigurationBuilder().SetBasePath(contentRoot)
                            .AddJsonFile("connectionstrings.json", true, true)))
                .UseSetting(WebHostDefaults.ApplicationKey, typeof(Program).GetTypeInfo().Assembly.FullName).Build();

            host.Run();
        }
    }
}
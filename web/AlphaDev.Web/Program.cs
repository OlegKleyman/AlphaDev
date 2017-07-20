using System.IO;
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
                .UseStartup<Startup>().UseApplicationInsights().ConfigureServices(
                    services => services.AddSingleton<IConfigurationBuilder, IConfigurationBuilder>(
                        provider => new ConfigurationBuilder().SetBasePath(contentRoot)
                            .AddJsonFile("connectionstrings.json", true, true))).Build();

            host.Run();
        }
    }
}
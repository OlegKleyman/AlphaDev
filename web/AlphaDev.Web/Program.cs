using System.IO;
using System.Reflection;
using AlphaDev.Web.Bootstrap;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AlphaDev.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            WebHost.CreateDefaultBuilder().UseStartup<Startup>().ConfigureServices(
                    services => services.AddSingleton<IConfigurationBuilder, IConfigurationBuilder>(
                        provider => new ConfigurationBuilder()
                            .AddJsonFile("connectionstrings.json", true, true)))
                .UseSetting(WebHostDefaults.ApplicationKey, typeof(Program).GetTypeInfo().Assembly.FullName).Build()
                .Run();
        }
    }
}
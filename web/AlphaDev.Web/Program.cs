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
            WebHost.CreateDefaultBuilder().UseStartup<Startup>()
                .ConfigureAppConfiguration(builder => builder.AddJsonFile("connectionstrings.json", true, true))
                .UseSetting(WebHostDefaults.ApplicationKey, typeof(Program).GetTypeInfo().Assembly.FullName).Build()
                .Run();
        }
    }
}
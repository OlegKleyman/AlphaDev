using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AlphaDev.Web.Tests.Integration.Fixtures
{
    public class WebSite : IDisposable
    {
        private IWebHost _host;
        public string Url { get; private set; }

        public void Dispose()
        {
            _host?.Dispose();
        }

        public void Start(string connectionString)
        {
            var url = $"http://127.0.0.1:{GetOpenPort()}";

            _host = new WebHostBuilder()
                .UseContentRoot(Path.GetFullPath(@"..\..\..\..\..\..\web\AlphaDev.Web")).UseKestrel().ConfigureServices(
                    services =>
                    {
                        services.AddSingleton<IConfigurationBuilder, IConfigurationBuilder>(
                            provider => new ConfigurationBuilder().SetBasePath(Path.GetFullPath("."))
                                .AddInMemoryCollection(new[]
                                {
                                    new KeyValuePair<string, string>("connectionStrings:default",
                                        connectionString)
                                }));
                    }).UseStartup<Startup>().UseUrls(url).Build();
            _host.Start();
            Url = url;
        }

        private int GetOpenPort()
        {
            using (var sock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
            {
                const int randomOpenPort = 0;

                sock.Bind(new IPEndPoint(IPAddress.Loopback, randomOpenPort));

                return ((IPEndPoint) sock.LocalEndPoint).Port;
            }
        }
    }
}
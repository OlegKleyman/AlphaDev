using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using AlphaDev.Web.Bootstrap;
using FluentAssertions.Common;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace AlphaDev.Web.Tests.Integration.Fixtures
{
    public class WebSite : IDisposable
    {
        static WebSite()
        {
            const string logTraceListenerName = "alpha_dev_integration_log";
            
            if (!(Trace.Listeners[logTraceListenerName] is TextWriterTraceListener traceListener) ||
                !(traceListener.Writer is StringWriter))
            {
                Trace.Listeners.Remove(logTraceListenerName);
                LogWriter = new StringWriter();
                Trace.Listeners.Add(new TextWriterTraceListener(LogWriter)
                {
                    Name = logTraceListenerName
                });
            }
        }

        private IWebHost _host;
        private static readonly StringWriter LogWriter;
        public string Url { get; private set; }

        public string Log => LogWriter.ToString();

        public void Dispose()
        {
            LogWriter.GetStringBuilder().Clear();
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
                    }).UseStartup<Startup>().UseUrls(url).UseSetting(WebHostDefaults.ApplicationKey,
                    typeof(Program).GetTypeInfo().Assembly.FullName).Build();

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
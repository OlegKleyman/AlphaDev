using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using AlphaDev.Core.Data.Account.Security.Sql.Entities;
using AlphaDev.Web.Bootstrap;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;

namespace AlphaDev.Web.Tests.Integration.Fixtures
{
    public class WebServer
    {
        private static readonly StringWriter LogWriter;

        private IWebHost _host;
        private readonly Dictionary<string, string> _connectionStrings;

        static WebServer()
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

        public WebServer(Dictionary<string, string> connectionStrings)
        {
            _connectionStrings = connectionStrings;
        }

        public IServiceProvider Start()
        {
            var url = $"http://127.0.0.1:{GetOpenPort()}";

            _host = new WebHostBuilder().ConfigureAppConfiguration(builder => builder.SetBasePath(Path.GetFullPath("."))
                    .AddJsonFile("appsettings.json", true, true)
                    .AddInMemoryCollection(_connectionStrings.Select(pair => new KeyValuePair<string, string>($"connectionStrings:{pair.Key}", pair.Value))))
                .UseContentRoot(Path.GetFullPath(@"..\..\..\..\..\..\web\AlphaDev.Web")).UseKestrel()
                .UseStartup<Startup>().UseUrls(url).UseSetting(WebHostDefaults.ApplicationKey,
                    typeof(Program).GetTypeInfo().Assembly.FullName).Build();

            _host.Start();

            Url = url;

            return _host.Services;
        }

        public string Url { get; private set; }

        public string Log => LogWriter.ToString();

        public void Dispose()
        {
            _host?.Dispose();
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

        public void ClearLog()
        {
            LogWriter.GetStringBuilder().Clear();
        }
    }
}
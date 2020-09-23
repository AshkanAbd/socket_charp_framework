using System;
using System.IO;
using socket_sharp.Core.Routing;
using socket_sharp.Core.Socket;
using Microsoft.Extensions.Configuration;

namespace socket_sharp.Core
{
    public class Startup
    {
        private IConfigurationRoot Configuration { get; set; }
        private Router Router { get; set; }
        private Server socketServer { get; set; }

        public Startup(Program program)
        {
            LoadConfig();
            SetupRouting();
            StartServer();
            program.Configure(socketServer);
            AcceptConnections();
        }

        private void LoadConfig()
        {
            try {
                var builder = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json");
                Configuration = builder.Build();
            }
            catch (Exception e) {
                throw new Exception("Can't find appsettings.json in launch folder.");
            }
        }

        private void SetupRouting()
        {
            Router = new Router();
            Routes.Routes.Register(Router);
        }

        private void StartServer()
        {
            socketServer = new Server(Configuration["SocketConfig:IP"], int.Parse(Configuration["SocketConfig:Port"]),
                Router);
        }

        private void AcceptConnections()
        {
            socketServer.Accept();
        }
    }
}
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using socket_sharp.Core.Routing;
using socket_sharp.Core.Socket;

namespace socket_sharp.Core
{
    public class Startup
    {
        private IConfigurationRoot Configuration { get; set; }
        private Router Router { get; set; }
        private Server SocketServer { get; set; }
        private IServiceCollection Services;
        private DbContext DbContext;

        public Startup(Program program)
        {
            LoadConfig();
            SetupRouting();
            StartServer();
            LoadAppConfiguration(program);
            AcceptConnections();
        }

        private void LoadAppConfiguration(Program program)
        {
            Services = new ServiceCollection();
            program.ConfigureServer(SocketServer);
            program.ConfigureServices(Services, Configuration);
            var serviceProvider = Services.BuildServiceProvider();
            DbContext = serviceProvider.GetService<DbContext>();
        }

        private void LoadConfig()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");
            Configuration = builder.Build();
        }

        private void SetupRouting()
        {
            Router = new Router(DbContext);
            Routes.Routes.Register(Router);
        }

        private void StartServer()
        {
            SocketServer = new Server(Configuration["SocketConfig:IP"], int.Parse(Configuration["SocketConfig:Port"]),
                Router);
        }

        private void AcceptConnections()
        {
            SocketServer.Accept();
        }
    }
}
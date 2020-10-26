using System;
using System.Net;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using socket_sharp.Core;
using socket_sharp.Core.Socket;
using DbContext = socket_sharp.Models.DbContext;

namespace socket_sharp
{
    public class Program
    {
        public static void Main()
        {
            new Startup(new Program());
        }

        public void ConfigureServices(IServiceCollection services, IConfigurationRoot configuration)
        {
            services.AddDbContext<DbContext>(options => {
                options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"));
                if (configuration["Environment"].Equals("Development")) {
                    options.EnableSensitiveDataLogging();
                }
            });
        }

        public void ConfigureServer(Server server)
        {
            server.SetOnClientConnect(client => {
                var endPoint = (IPEndPoint) client.ClientSocket.RemoteEndPoint;
                Console.WriteLine("{0}:{1} connected", endPoint.Address, endPoint.Port);
                return true;
            });
            server.SetOnMessageReceive((client, buffer) => {
                Console.Write("Byte array: ");
                var strArr = new string[buffer.Length];
                for (var i = 0; i < buffer.Length; i++) {
                    strArr[i] = buffer[i].ToString("X");
                    Console.Write(buffer[i]);
                    Console.Write(", ");
                }

                Console.WriteLine();

                Console.Write("Hex String: ");
                Array.ForEach(strArr, s => Console.Write(s + ", "));
                Console.WriteLine();

                Console.WriteLine("String: " + Encoding.Default.GetString(buffer));
                Console.WriteLine("#################################");
                return true;
            });
        }
    }
}
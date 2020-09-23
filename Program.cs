using System;
using socket_sharp.Core;
using socket_sharp.Core.Socket;

namespace socket_sharp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            new Startup(new Program());
        }

        public void Configure(Server server)
        {
            server.SetOnClientConnect(OnClientConnected);
            server.SetOnMessageReceive(OnMessageReceive);
        }

        private bool OnClientConnected(Client socketClient)
        {
            return true;
        }

        private bool OnMessageReceive(Client client, byte[] buffer)
        {
            return true;
        }
    }
}
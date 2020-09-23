using System;
using System.Net;
using System.Net.Sockets;
using socket_sharp.Core.Routing;

namespace socket_sharp.Core.Socket
{
    public class Server
    {
        private readonly TcpListener _socketServer;
        private readonly Router _router;

        private Func<Client, bool> OnClientConnect;
        private Func<Client, byte[], bool> OnMessageReceive;

        public Server(string ip, int port, Router router)
        {
            _router = router;
            var ipAddress = IPAddress.Parse(ip);
            _socketServer = new TcpListener(ipAddress, port);
            _socketServer.Start();
            Console.WriteLine("Socket created on port " + port);
        }

        public void Accept()
        {
            try {
                while (true) {
                    var tcpClient = _socketServer.AcceptTcpClient();
                    var client = new Client(tcpClient, tcpClient.Client, tcpClient.GetStream(), _router);
                    OnClientConnect.Invoke(client);
                    client.SetOnMessageReceive(OnMessageReceive);
                }
            }
            catch (SocketException e) {
                Console.WriteLine("SocketException: {0}", e);
            }
        }

        public void SetOnClientConnect(Func<Client, bool> onClientConnect)
        {
            OnClientConnect = onClientConnect;
        }

        public void SetOnMessageReceive(Func<Client, byte[], bool> onMessageReceive)
        {
            OnMessageReceive = onMessageReceive;
        }
    }
}
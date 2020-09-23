using System;
using System.Net.Sockets;
using System.Threading;
using socket_sharp.Core.Routing;

namespace socket_sharp.Core.Socket
{
    public class Client
    {
        public TcpClient TcpClient { get; }
        public System.Net.Sockets.Socket ClientSocket { get; }
        public NetworkStream ClientIoStream { get; }

        private Func<Client, byte[], bool> OnMessageReceive;
        private readonly Router _router;

        public Client(TcpClient client, System.Net.Sockets.Socket clientSocket, NetworkStream clientIoStream,
            Router router)
        {
            TcpClient = client;
            ClientSocket = clientSocket;
            ClientIoStream = clientIoStream;
            _router = router;

            var clientThread = new Thread(HandleIncomingMessage);
            clientThread.Start();
        }

        public Client(TcpClient tcpClient, System.Net.Sockets.Socket clientSocket, NetworkStream clientIoStream)
        {
            TcpClient = tcpClient;
            ClientSocket = clientSocket;
            ClientIoStream = clientIoStream;

            var clientThread = new Thread(HandleIncomingMessage);
            clientThread.Start();
        }

        public async void SendMessage(byte[] buffer)
        {
            await ClientIoStream.WriteAsync(buffer);
        }

        private void HandleIncomingMessage()
        {
            try {
                const int len = 1024 * 1024;
                var buffer = new byte[len];
                int count;
                while ((count = ClientIoStream.Read(buffer, 0, len)) != 0) {
                    var tmp = new byte[count];
                    Array.Copy(buffer, tmp, count);
                    OnMessageReceive.Invoke(this, tmp);
                    _router?.GetTarget(tmp)?.Invoke(this, tmp);
                }
            }
            catch (Exception e) {
                Console.WriteLine("Exception: {0}", e);
            }
        }

        public void SetOnMessageReceive(Func<Client, byte[], bool> onMessageReceive)
        {
            OnMessageReceive = onMessageReceive;
        }
    }
}
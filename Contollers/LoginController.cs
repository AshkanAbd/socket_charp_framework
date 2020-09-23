using socket_sharp.Core.Socket;

namespace socket_sharp.Contollers
{
    public class LoginController
    {
        public byte[] Login(Client client, byte[] buffer)
        {
            return new byte[] {0, 0, 0};
        }
    }
}
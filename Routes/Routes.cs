using socket_sharp.Contollers;
using socket_sharp.Core.Routing;

namespace socket_sharp.Routes
{
    public class Routes
    {
        public static void Register(Router router)
        {
            router.On("78,x,0A", 1).To(new LoginController().Login);
        }
    }
}
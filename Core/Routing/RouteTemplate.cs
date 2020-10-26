using System;
using System.Text;
using socket_sharp.Core.Controller;
using socket_sharp.Core.Socket;

namespace socket_sharp.Core.Routing
{
    public class RouteTemplate
    {
        public byte[] StartBits { get; set; }
        public byte[] EndBits { get; set; }
        public byte[] ProtocolNumber { get; set; }
        public byte packetLength { get; set; }

        public Router Router { get; set; }

        public Router To(Func<Client, byte[], object> action)
        {
            if (action.Target is ControllerBase controller) {
                controller.DbContext = Router.DbContext;
            }

            if (!Router.registerRoutes.TryAdd(this, action)) {
                throw new Exception($"Can't add the router {this}:{action}");
            }

            return Router;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is RouteTemplate)) {
                return false;
            }

            var tmp = (RouteTemplate) obj;
            return tmp.StartBits == StartBits && tmp.packetLength == packetLength &&
                   tmp.ProtocolNumber == ProtocolNumber && tmp.EndBits == EndBits && tmp.Router == Router;
        }

        public override int GetHashCode()
        {
            return StartBits.GetHashCode() * ProtocolNumber.GetHashCode() *
                   packetLength.GetHashCode() * EndBits.GetHashCode() * Router.GetHashCode();
        }

        public override string ToString()
        {
            var builder = new StringBuilder();
            builder.Append("StartBits: ");
            Array.ForEach(StartBits, b => builder.Append(b));
            builder.Append(", PackagePlaceholder: ");
            builder.Append(packetLength);
            builder.Append(", ProtocolNumber: ");
            Array.ForEach(ProtocolNumber, b => builder.Append(b));
            builder.Append(", EndBits: ");
            Array.ForEach(EndBits, b => builder.Append(b));
            return builder.ToString();
        }
    }
}
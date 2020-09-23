using System;
using System.Collections.Generic;
using System.Globalization;
using socket_sharp.Core.Socket;

namespace socket_sharp.Core.Routing
{
    public class Router
    {
        public readonly Dictionary<RouteTemplate, Func<Client, byte[], byte[]>> registerRoutes;

        public Router()
        {
            registerRoutes = new Dictionary<RouteTemplate, Func<Client, byte[], byte[]>>();
        }

        public Func<Client, byte[], byte[]> GetTarget(byte[] buffer)
        {
            Func<Client, byte[], byte[]> targetFunc = null;

            foreach (var (template, target) in registerRoutes) {
                for (var i = 0; i < template.StartBits.Length; i++) {
                    if (buffer[i] != template.StartBits[i]) {
                        goto nextTemplate;
                    }
                }

                for (var i = 0; i < template.EndBits.Length; i++) {
                    if (template.EndBits[i] != buffer[buffer.Length - template.EndBits.Length + i]) {
                        goto nextTemplate;
                    }
                }

                for (var i = 0; i < template.ProtocolNumber.Length; i++) {
                    if (buffer[template.StartBits.Length + template.PackagePlaceholder + i] !=
                        template.ProtocolNumber[i]) {
                        goto nextTemplate;
                    }
                }

                return target;

                nextTemplate:
                {
                }
            }

            return null;
        }

        public RouteTemplate On(byte[] startBits, byte packagePlaceholder, byte[] protocolNumber, byte[] endBites)
        {
            return new RouteTemplate {
                StartBits = startBits,
                ProtocolNumber = protocolNumber,
                PackagePlaceholder = packagePlaceholder,
                EndBits = endBites,
                Router = this
            };
        }

        public RouteTemplate On(string template, byte packagePlaceholder)
        {
            var splitTemplate = template.Split("x");
            var routeTemplate = new RouteTemplate {
                Router = this,
                PackagePlaceholder = packagePlaceholder,
            };
            if (splitTemplate.Length != 3) {
                throw new Exception($"Invalid route {template}.");
            }

            for (var i = 0; i < splitTemplate.Length; i++) {
                var stringTemplate = splitTemplate[i];
                if (splitTemplate[i].StartsWith(",")) {
                    stringTemplate = stringTemplate.Substring(1);
                }

                if (splitTemplate[i].EndsWith(",")) {
                    stringTemplate = stringTemplate.Substring(0, stringTemplate.Length - 1);
                }

                var hexString = stringTemplate.Split(",");
                if (hexString.Length == 0) {
                    throw new Exception($"Invalid route {template}.");
                }

                var bytes = new byte[hexString.Length];
                for (var j = 0; j < hexString.Length; j++) {
                    bytes[j] = byte.Parse(hexString[j], NumberStyles.HexNumber);
                }

                switch (i) {
                    case 0:
                        routeTemplate.StartBits = bytes;
                        break;
                    case 1:
                        routeTemplate.ProtocolNumber = bytes;
                        break;
                    case 2:
                        routeTemplate.EndBits = bytes;
                        break;
                }
            }

            return routeTemplate;
        }
    }
}
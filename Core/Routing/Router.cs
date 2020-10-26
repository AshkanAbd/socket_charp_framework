using System;
using System.Collections.Generic;
using System.Globalization;
using Microsoft.EntityFrameworkCore;
using socket_sharp.Core.Socket;

namespace socket_sharp.Core.Routing
{
    public class Router
    {
        public readonly Dictionary<RouteTemplate, Func<Client, byte[], object>> registerRoutes;
        public readonly DbContext DbContext;

        public Router(DbContext dbContext)
        {
            registerRoutes = new Dictionary<RouteTemplate, Func<Client, byte[], object>>();
            DbContext = dbContext;
        }

        public Func<Client, byte[], object> GetTarget(byte[] buffer)
        {
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
                    if (buffer[template.StartBits.Length + template.packetLength + i] !=
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

        public RouteTemplate On(byte[] startBits, byte packetLength, byte[] protocolNumber, byte[] endBites)
        {
            return new RouteTemplate {
                StartBits = startBits,
                ProtocolNumber = protocolNumber,
                packetLength = packetLength,
                EndBits = endBites,
                Router = this
            };
        }

        public RouteTemplate On(string template, byte packetLength)
        {
            var splitTemplate = template.Split("x");
            var routeTemplate = new RouteTemplate {
                Router = this,
                packetLength = packetLength,
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
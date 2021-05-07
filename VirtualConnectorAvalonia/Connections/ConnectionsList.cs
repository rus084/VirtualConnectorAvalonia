using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualConnectorAvalonia.Connections
{
    static class ConnectionsList
    {
        public static IEnumerable<IConnection> GetConnections()
        {
            return new IConnection[]
            {
                new COMPort(),
                new TCPclient(),
                new TCPserver(),
            };
        }
    }
}

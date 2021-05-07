using System;
using System.Collections.Generic;
using System.Text;
using VirtualConnectorAvalonia.Connections.Params;

namespace VirtualConnectorAvalonia.Connections
{
    public interface IConnection
    {
        public IConnection GetNewInstance();
        public void SetCallbackHandler(IConnector CallBacksHandler);
        IReadOnlyCollection<ConnectionParam> GetParams();
        public bool SetParam(int param, object value);
        public void Open();
        public void Close();
        public void Send(byte[] data);
        public string Name { get; }
        public string Info { get; }
    }
}

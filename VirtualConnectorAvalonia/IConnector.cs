using VirtualConnectorAvalonia.Connections;

namespace VirtualConnectorAvalonia
{
    public interface IConnector
    {
        public void Receive(IConnection sender, byte[] data);
        public void AddConnection(IConnection connection);
        public void ConnectionClosedCallBack(IConnection sender);
    }
}

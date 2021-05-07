using Avalonia.Controls;
using VirtualConnectorAvalonia.Connections;

namespace VirtualConnectorAvalonia.UI.Connections
{
    public class ConnectionCreatorWrapper : IConnectionCreator
    {
        private ChanelParamsControl control;
        public override string ToString()
        {
            return control.ConnectionType;
        }

        public IControl Control { get => control; }

        public IConnection CreateConnection(IConnector network)
        {
            return control.CreateConnection(network);
        }

        public ConnectionCreatorWrapper(IConnection connection)
        {
            control = new ChanelParamsControl(connection);
        }
    }
}

using Avalonia.Controls;
using VirtualConnectorAvalonia.Connections;

namespace VirtualConnectorAvalonia.UI.Connections
{
    public interface IConnectionCreator
    {
        public IControl Control { get; }
        public IConnection CreateConnection(IConnector network);
    }
}

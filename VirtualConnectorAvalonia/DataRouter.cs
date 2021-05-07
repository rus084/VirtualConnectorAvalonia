using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using VirtualConnectorAvalonia.Connections;

namespace VirtualConnectorAvalonia
{
    public class DataRouter : IConnector
    {
        public ObservableCollection<IConnection> connections;


        public string Name { get; set; }

        public DataRouter(string name)
        {
            Name = name;
            connections = new ObservableCollection<IConnection>();
        }

        public void Receive(IConnection sender, byte[] data)
        {
            foreach (var connection in connections)
            {
                if (connection != sender)
                {
                    connection.Send(data);
                }
            }
        }

        public void ConnectionClosedCallBack(IConnection sender)
        {
            try
            {
                Avalonia.Threading.Dispatcher.UIThread.InvokeAsync(() =>
                {
                    connections.Remove(sender);
                });
                //ConnectionsUpdated?.Invoke(this, sender);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }


        public void AddConnection(IConnection connection)
        {
            Avalonia.Threading.Dispatcher.UIThread.InvokeAsync(() =>
            {
                connections.Add(connection);
            });
            //ConnectionsUpdated?.Invoke(this, connection);
        }

        public void Destroy()
        {
            List<IConnection> currentConnections = new List<IConnection>(connections);
            foreach (var connection in currentConnections)
            {
                connection.Close();
            }
        }
    }
}

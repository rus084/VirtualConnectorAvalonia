using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using VirtualConnectorAvalonia.Connections.Params;

namespace VirtualConnectorAvalonia.Connections
{
    class TCPserver : IConnection
    {
        IConnector _CallBacksHandler;
        Socket socket;

        int port;

        private void AcceptCallback(IAsyncResult ar)
        {
            // Get the socket that handles the client request.  
            try
            {
                object? AsyncState = ar.AsyncState;
                if (AsyncState == null)
                    throw new Exception("Socket in TCP server was null");
                Socket listener = (Socket)AsyncState;
                Socket handler = listener.EndAccept(ar);

                IConnection connection = new TCPclient(_CallBacksHandler, handler);

                _CallBacksHandler.AddConnection(connection);

                socket.BeginAccept(
                   new AsyncCallback(AcceptCallback),
                   socket);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                _CallBacksHandler?.ConnectionClosedCallBack(this);
                Close();
            }



        }

        public TCPserver()
        {
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }

        public void Open()
        {
            IPEndPoint ipPoint = new IPEndPoint(IPAddress.Any, port);

            socket.Bind(ipPoint);
            socket.Listen(10);
            socket.BeginAccept(
                   new AsyncCallback(AcceptCallback),
                   socket);
        }

        public void Close()
        {
            socket.Close();
        }

        public void Send(byte[] data)
        {
        }

        public bool SetParam(int param, object value)
        {
            try
            {
                switch (param)
                {
                    case 0:
                        port = int.Parse((string)value);
                        break;
                    default:
                        throw new Exception("Param in not exist");

                }
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return false;
            }
        }

        public IConnection GetNewInstance()
        {
            return new TCPserver();
        }

        public void SetCallbackHandler(IConnector CallBacksHandler)
        {
            _CallBacksHandler = CallBacksHandler;
        }

        static ConnectionParam[] connectionParams = new ConnectionParam[]
        {
            new StringParam(0,"Port", "8000"),
        };

        public IReadOnlyCollection<ConnectionParam> GetParams()
        {
            return connectionParams;
        }

        public string Info { 
            get
            {
                return "TCP server: " + socket.LocalEndPoint?.ToString();
            }
        }

        public string Name { 
            get 
            {
                return "TCP Server";
            }
        }
    }
}

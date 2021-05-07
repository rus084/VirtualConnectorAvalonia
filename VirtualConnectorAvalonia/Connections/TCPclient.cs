using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using VirtualConnectorAvalonia.Connections.Params;

namespace VirtualConnectorAvalonia.Connections
{
    class TCPclient : IConnection
    {
        object locker;
        Socket socket;
        IConnector _CallBacksHandler;
        byte[] receiverBuffer;

        string host;
        int port;

        private void ReceiveCallback(IAsyncResult ar)
        {
            try
            {
                // Read data from the remote device.  
                int bytesRead = socket.EndReceive(ar);

                if (bytesRead > 0)
                {
                    byte[] temp = new byte[bytesRead];
                    Array.Copy(receiverBuffer, temp, bytesRead);
                    _CallBacksHandler.Receive(this, temp);

                    // Get the rest of the data.  
                    socket.BeginReceive(receiverBuffer, 0, receiverBuffer.Length, 0,
                       new AsyncCallback(ReceiveCallback), null);
                }
                else
                {
                    _CallBacksHandler.ConnectionClosedCallBack(this);
                    Close();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                _CallBacksHandler.ConnectionClosedCallBack(this);
                Close();
            }
        }

        public TCPclient()
        {
            this.host = "";
            this.port = 0;
            locker = new object();
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            receiverBuffer = new byte[1024];
        }

        public bool SetParam(int param, object value)
        {
            try
            {
                switch (param)
                {
                    case 0:
                        string ipAddr = (string)value;
                        IPAddress.Parse(ipAddr);
                        host = ipAddr;
                        break;
                    case 1:
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
            return new TCPclient();
        }

        public void Open()
        {
            IPEndPoint ipPoint = new IPEndPoint(IPAddress.Parse(host), port);
            socket.Connect(ipPoint);
            socket.BeginReceive(receiverBuffer, 0, receiverBuffer.Length, 0,
               new AsyncCallback(ReceiveCallback), null);
        }

        public TCPclient(IConnector CallBacksHandler, Socket connection)
        {
            this.host = "";
            this.port = 0;
            _CallBacksHandler = CallBacksHandler;
            locker = new object();
            socket = connection;
            receiverBuffer = new byte[1024];

            socket.BeginReceive(receiverBuffer, 0, receiverBuffer.Length, 0,
               new AsyncCallback(ReceiveCallback), null);
        }

        public void Close()
        {
            socket.Close();
        }

        public void Send(byte[] data)
        {
            lock (locker)
            {
                try
                {
                    socket.Send(data);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                    _CallBacksHandler.ConnectionClosedCallBack(this);
                    Close();
                }
            }
        }

        public void SetCallbackHandler(IConnector CallBacksHandler)
        {
            _CallBacksHandler = CallBacksHandler;
        }


        static ConnectionParam[] connectionParams = new ConnectionParam[]
        {
            new StringParam(0,"Address", "127.0.0.1"),
            new StringParam(1,"Port", "8000"),
        };

        public IReadOnlyCollection<ConnectionParam> GetParams()
        {
            return connectionParams;
        }

        public string Info
        {
            get
            {
                return "TCP socket: " + socket.LocalEndPoint?.ToString() + " <-> " + socket.RemoteEndPoint?.ToString();
            }
        }

        public string Name
        {
            get
            {
                return "TCP Client";
            }
        }
    }
}

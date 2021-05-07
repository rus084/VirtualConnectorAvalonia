using System;
using System.Collections.Generic;
using System.Text;
using System.IO.Ports;
using System.Threading;
using VirtualConnectorAvalonia.Connections.Params;

namespace VirtualConnectorAvalonia.Connections
{
    public class COMPort : IConnection
    {
        SerialPort port;
        object locker;

        IConnector _CallBacksHandler;

        public COMPort()
        {
            locker = new object();
            port = new SerialPort();
            port.ReceivedBytesThreshold = 1;
            port.DataReceived += Port_DataReceived;
            port.ErrorReceived += Port_ErrorReceived;
        }

        public void Open()
        {
            port.Open();
        }

        private void Port_ErrorReceived(object sender, SerialErrorReceivedEventArgs e)
        {
            _CallBacksHandler.ConnectionClosedCallBack(this);
            Close();
        }

        private void Port_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {

            int bufferLength = port.BytesToRead;
            byte[] buffer = new byte[bufferLength];
            try
            {
                int length = port.Read(buffer, 0, bufferLength);
                byte[] temp = new byte[length];
                Array.Copy(buffer, temp, length);
                _CallBacksHandler.Receive(this, temp);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                _CallBacksHandler.ConnectionClosedCallBack(this);
                Close();
            }
        }

        public void Close()
        {
            port.Close();
            _CallBacksHandler.ConnectionClosedCallBack(this);
            port.Dispose();
        }

        public void Send(byte[] data)
        {
            lock (locker)
            {
                try
                {
                    port.Write(data, 0, data.Length);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                    _CallBacksHandler.ConnectionClosedCallBack(this);
                    Close();
                }
            }
        }

        public bool SetParam(int param, object value)
        {
            try
            {
                switch (param)
                {
                    case 0:
                        port.PortName = (string)value;
                        break;
                    case 1:
                        port.BaudRate = int.Parse((string)value);
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
            return new COMPort();
        }

        public void SetCallbackHandler(IConnector CallBacksHandler)
        {
            _CallBacksHandler = CallBacksHandler;
        }


        static ConnectionParam[] connectionParams = new ConnectionParam[]
        {
            new DynamicEnumParam(
                paramNumber: 0,
                paramName: "Port",
                validValuesGetter: () => SerialPort.GetPortNames(),
                defaultIndex: 0
            ),
            new StaticEnumParam(
                paramNumber: 1,
                paramName: "baud rate",
                validValues: new[]
                {
                    "1200",
                    "2400",
                    "4800",
                    "9600",
                    "19200",
                    "38400",
                    "57600",
                    "115200",
                },
                defaultIndex: 3
            ),
        };

        public IReadOnlyCollection<ConnectionParam> GetParams()
        {
            return connectionParams;
        }

        public string Info
        {
            get
            {
                return "COM port: " + port.PortName + ":" + port.BaudRate.ToString();
            }
        }

        public string Name
        {
            get
            {
                return "COM Port";
            }
        }
    }
}

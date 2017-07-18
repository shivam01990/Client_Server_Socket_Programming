using CommonLib.Adapter.Communication.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace CommonLib.Adapter
{
    public class SocketAdapter : ISocketAdapter, IDisposable
    {
        #region Constants
        /// <summary>
        /// Port Number
        /// </summary>
        private const string PortNumber = "PNO#!";

        /// <summary>
        /// FTI Device Name
        /// </summary>
        private const string FTIDeviceName = "DNAME#!";

        /// <summary>
        /// Staring Port Number
        /// </summary>
        private const int StaringPortNumber = 12345;
        #endregion

        /// <summary>
        /// Sender socket
        /// </summary>
        private Socket senderSock;

        /// <summary>
        /// Port number
        /// </summary>
        private int portNumber;

        /// <summary>
        /// Initializes a new instance of the <see cref="SocketAdapter" /> class.
        /// </summary>
        /// <param name="portNo">port no</param>
        public SocketAdapter(int portNo)
        {
            this.portNumber = portNo;
        }

        /// <summary>
        /// Response Delegate
        /// </summary>
        /// <param name="incomingData">incoming data</param>
        public delegate void ResponseRecievedDelegate(string incomingData);

        /// <summary>
        /// On Response Received
        /// </summary>
        public event ResponseRecievedDelegate OnResponseReceived;

        /// <summary>
        /// Ping Available Server Sockets
        /// </summary>
        /// <returns>Tuple list</returns>
        public static IList<Tuple<string, string>> PingAvailableServerSockets()
        {
            var tmpPortNo = StaringPortNumber;
            var lstPortNoAndDevice = new List<Tuple<string, string>>();
            var parallerOptions = new ParallelOptions();
            parallerOptions.MaxDegreeOfParallelism = 4;

            Parallel.For(StaringPortNumber, 12355, parallerOptions, PingOpenedSockets(ref tmpPortNo, lstPortNoAndDevice));

            return lstPortNoAndDevice;
        }

        /// <summary>
        /// Close method
        /// </summary>
        public void Close()
        {
            this.senderSock.Shutdown(SocketShutdown.Both);
            this.senderSock.Close();
        }

        /// <summary>
        /// Connect Method
        /// </summary>
        public void Connect()
        {
            var permission = new SocketPermission(NetworkAccess.Connect, TransportType.Tcp, string.Empty, SocketPermission.AllPorts);
            permission.Demand();

            IPHostEntry ipHost = Dns.GetHostEntry(string.Empty);

            IPAddress ipAddr = ipHost.AddressList[0];
            IPEndPoint ipEndPoint = new IPEndPoint(ipAddr, this.portNumber);

            this.senderSock = new Socket(ipAddr.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            this.senderSock.NoDelay = false;

            this.senderSock.Connect(ipEndPoint);
        }

        /// <summary>
        /// Send Data
        /// </summary>
        /// <param name="content">content string</param>
        public void SendData(string content)
        {
            var msg = Encoding.Unicode.GetBytes(content);
            int bytesSend = this.senderSock.Send(msg);
            this.ReceiveData();
        }

        /// <summary>
        /// Disposed Method
        /// </summary>
        public void Dispose()
        {
            this.Close();
        }

        /// <summary>
        /// To Identify the Available Sockets of Emulator
        /// </summary>
        /// <param name="tmpPortNo">POrt Number which needs to be Pinged</param>
        /// <param name="lstPortNoAndDevice">List that has the device Info and Port Number</param>
        /// <returns>Action Method</returns>
        private static Action<int> PingOpenedSockets(ref int tmpPortNo, List<Tuple<string, string>> lstPortNoAndDevice)
        {
            return portNumber =>
            {
                try
                {
                    var permission = new SocketPermission(NetworkAccess.Connect, TransportType.Tcp, string.Empty, SocketPermission.AllPorts);
                    permission.Demand();

                    IPHostEntry ipHost = Dns.GetHostEntry(string.Empty);

                    IPAddress ipAddr = ipHost.AddressList[0];
                    IPEndPoint ipEndPoint = new IPEndPoint(ipAddr, portNumber);

                    Socket ssock = new Socket(ipAddr.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                    ssock.NoDelay = false;

                    ssock.Connect(ipEndPoint);

                    ////Send the Reserved Commands to Pull Device Info

                    var msg = Encoding.Unicode.GetBytes(FTIDeviceName);
                    int bytesSend = ssock.Send(msg);

                    ////Receive the bytes from server and watch
                    var bytes = new byte[1024];
                    var bytesRec = ssock.Receive(bytes);
                    string recievedContent = Encoding.Unicode.GetString(bytes, 0, bytesRec);

                    while (ssock.Available > 0)
                    {
                        bytesRec = ssock.Receive(bytes);
                        recievedContent += Encoding.Unicode.GetString(bytes, 0, bytesRec);
                    }

                    lstPortNoAndDevice.Add(new Tuple<string, string>(portNumber.ToString(), recievedContent));
                    ssock.Shutdown(SocketShutdown.Both);
                    ssock.Close();

                    ////tmpPortNo++;
                }
                catch
                {
                    ////tmpPortNo++;
                }
            };
        }

        /// <summary>
        /// Receive Data
        /// </summary>
        private void ReceiveData()
        {
            string recievedContent = this.BuildResponse();
            if (!string.IsNullOrEmpty(recievedContent))
            {
                this.OnResponseReceived?.Invoke(recievedContent);
            }
        }

        /// <summary>
        /// Build method
        /// </summary>
        /// <returns>string type</returns>
        private string BuildResponse()
        {
            var bytes = new byte[1024];
            var bytesRec = this.senderSock.Receive(bytes);
            string recievedContent = Encoding.Unicode.GetString(bytes, 0, bytesRec);
            while (this.senderSock.Available > 0)
            {
                bytesRec = this.senderSock.Receive(bytes);
                recievedContent += Encoding.Unicode.GetString(bytes, 0, bytesRec);
            }

            return recievedContent;
        }
    }
}
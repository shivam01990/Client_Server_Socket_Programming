using CommonLib.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace CommonLib.Communication
{
    internal class SocketServer : IDisposable 
    {
        #region Constants

        /// <summary>
        /// back logs
        /// </summary>
        private const int BackLog = 0;

        /// <summary>
        /// Default Acknowledgement
        /// </summary>
        private const string DefaultAcknowledgement = "Acknowledged";
        #endregion

        #region Variables

        /// <summary>
        /// permission Socket
        /// </summary>
        private SocketPermission permission;

        /// <summary>
        /// Listener Socket
        /// </summary>
        private Socket slistener = null;

        /// <summary>
        /// IP End Point
        /// </summary>
        private IPEndPoint ipEndPoint;

        /// <summary>
        ///  Socket handler
        /// </summary>
        private Socket handler;

        /// <summary>
        /// Initializes a new instance of the <see cref="SocketServer" /> class.
        /// </summary>
        /// <param name="portNo">port no</param>
        /// <param name="deviceType">device type</param>
        public SocketServer(int portNo)
        {
            this.PortNumber = portNo;
        }

        /// <summary>
        /// Request Delegate
        /// </summary>
        /// <param name="incomingData">incoming data</param>
        internal delegate void RequestRecievedDelegate(string incomingData);

        /// <summary>
        /// On Request Received
        /// </summary>
        internal event RequestRecievedDelegate OnRequestReceived;
        #endregion

        /// <summary>
        /// Gets Port Number
        /// </summary>
        public int PortNumber { get; private set; }

        /// <summary>
        /// Close method
        /// </summary>
        public void Close()
        {
            if (this.slistener.Connected)
            {
                this.slistener.Shutdown(SocketShutdown.Both);
            }

            this.slistener.Close();
        }

        /// <summary>
        /// Listen to port
        /// </summary>
        public void ListenToPort()
        {
            this.slistener.Listen(BackLog);
            var acallback = new AsyncCallback(this.AcceptCallback);
            this.slistener.BeginAccept(acallback, this.slistener);
        }

        /// <summary>
        /// Open Port
        /// </summary>
        public void OpenPort()
        {
            const string SocketBusyMessage = "Selected Port is Busy";
            if (SocketHelper.IsPortAvailable(this.PortNumber))
            {
                throw new Exception(SocketBusyMessage);
            }

            this.permission = new SocketPermission(NetworkAccess.Accept, TransportType.Tcp, string.Empty, SocketPermission.AllPorts);
            this.permission.Demand();

            IPHostEntry ipHost = Dns.GetHostEntry(string.Empty);

            IPAddress ipAddr = ipHost.AddressList[0];
            this.ipEndPoint = new IPEndPoint(ipAddr, this.PortNumber);

            this.slistener = new Socket(ipAddr.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            this.slistener.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
            this.slistener.Bind(this.ipEndPoint);
        }

        /// <summary>
        /// Send Acknowledgement
        /// </summary>
        /// <param name="data">name data</param>
        public void SendAcknowledgement(string data = "")
        {
            data = string.IsNullOrEmpty(data) ? DefaultAcknowledgement : data;
            byte[] byteData = Encoding.Unicode.GetBytes(data);
            this.handler.BeginSend(byteData, 0, byteData.Length, 0, new AsyncCallback(this.SendCallback), this.handler);
        }

        /// <summary>
        /// Close Dispose
        /// </summary>
        public void Dispose()
        {
            this.Close();
        }

        /// <summary>
        /// Accept Callback
        /// </summary>
        /// <param name="ar"> Async Result </param>
        private void AcceptCallback(IAsyncResult ar)
        {
            try
            {
                byte[] buffer = new byte[1024];
                Socket listener = (Socket)ar.AsyncState;
                Socket handlerinstance = listener.EndAccept(ar);

                handlerinstance.NoDelay = false;

                object[] obj = new object[2];
                obj[0] = buffer;
                obj[1] = handlerinstance;

                handlerinstance.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, new AsyncCallback(this.ReceiveCallback), obj);

                AsyncCallback acallback = new AsyncCallback(this.AcceptCallback);
                listener.BeginAccept(acallback, listener);
            }
            catch
            {
                ////TODO: Log content of exception
            }
        }

        /// <summary>
        /// Receive Callback
        /// </summary>
        /// <param name="ar">Async Result</param>
        private void ReceiveCallback(IAsyncResult ar)
        {
            try
            {
                if (this.slistener.Available == 0)
                {
                    object[] obj = new object[2];
                    obj = (object[])ar.AsyncState;

                    byte[] buffer = (byte[])obj[0];

                    this.handler = (Socket)obj[1];

                    string content = string.Empty;

                    int bytesRead = this.handler.EndReceive(ar);

                    if (bytesRead > 0)
                    {
                        content += Encoding.Unicode.GetString(buffer, 0, bytesRead);

                        byte[] buffernew = new byte[1024];
                        obj[0] = buffernew;
                        obj[1] = this.handler;

                        this.OnRequestReceived?.Invoke(content);
                        this.handler.BeginReceive(buffernew, 0, buffernew.Length, SocketFlags.None, new AsyncCallback(this.ReceiveCallback), obj);
                    }
                }
            }
            catch
            {
                ////TODO: Need to Handle
                ////The above will raise if client close the socket abruptly and server is on recieving mode
            }
        }

        /// <summary>
        /// Send Callback
        /// </summary>
        /// <param name="ar">Async Result</param>
        private void SendCallback(IAsyncResult ar)
        {
            Socket handler = (Socket)ar.AsyncState;
            int bytesSend = this.handler.EndSend(ar);
        }
    }
}

using CommonLib.Communication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLib.Controller
{
    /// </summary>
    public abstract class BaseController
    {
        #region Constants

        /// <summary>
        /// Socket Server
        /// </summary>
        internal readonly SocketServer Server;

        /// <summary>
        /// Port Number
        /// </summary>
        protected const string PortNumber = "PNO#!";

        /// <summary>
        /// Device Name
        /// </summary>
        protected const string DeviceName = "DNAME#!";
        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseController" /> class.
        /// </summary>
        /// <param name="portNo">port no</param>
        /// <param name="deviceType">device type</param>
        protected BaseController(int portNo)
        {
            this.GetPortNo = portNo;
            this.Server = new SocketServer(portNo);
        }

        /// <summary>
        /// Request Receive Delegate
        /// </summary>
        /// <param name="incomingRequest">incoming Request</param>
        public delegate void RequestRecievedDelegate(string incomingRequest);

        /// <summary>
        /// LookUp Complete Delegate
        /// </summary>
        /// <param name="lookupValue">lookup Value</param>
        public delegate void LookUpCompleteDelegate(string lookupValue);

        /// <summary>
        /// On Request Received
        /// </summary>
        public event RequestRecievedDelegate OnRequestReceived;

        /// <summary>
        /// On LookUp Complete
        /// </summary>
        public event LookUpCompleteDelegate OnLookUpComplete;

        /// <summary>
        /// Gets Port No
        /// </summary>
        public int GetPortNo { get; private set; }


        /// <summary>
        /// Start Server
        /// </summary>
        public void StartServer()
        {
            this.Server.OpenPort();
            this.Server.ListenToPort();
        }

        /// <summary>
        /// Stop Server
        /// </summary>
        public void StopServer()
        {
            this.Server.Close();
        }

        /// <summary>
        /// Raise Event For LookUp Complete
        /// </summary>
        /// <param name="lookUpValue">lookup value</param>
        protected void RaiseEventForLookUpComplete(string lookUpValue)
        {
            this.OnLookUpComplete?.Invoke(lookUpValue);
        }

        /// <summary>
        /// Raise Event For Request Received
        /// </summary>
        /// <param name="incomingData">incoming data</param>
        protected void RaiseEventForRequestReceived(string incomingData)
        {
            switch (incomingData)
            {
                case PortNumber:
                    this.Server.SendAcknowledgement(this.GetPortNo.ToString());
                    break;
                case DeviceName:
                    this.Server.SendAcknowledgement("Device");
                    break;
                default:
                    this.OnRequestReceived?.Invoke(incomingData);
                    break;
            }
        }
    }
}

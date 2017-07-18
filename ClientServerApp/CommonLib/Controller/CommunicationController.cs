using CommonLib.Parser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLib.Controller
{
    public class CommunicationController : BaseController
    {
        #region Variables
        /// <summary>
        /// Base Parser
        /// </summary>
        private readonly BaseParser parser;
        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="CommunicationController" /> class.
        /// </summary>
        /// <param name="portNo">port no</param>       
        public CommunicationController(int portNo): base(portNo)
        {
            this.parser = new CommandParser();
            this.Server.OnRequestReceived += this.SServer_OnRequestReceived;
            this.parser.OnLookupComplete += this.EsaParser_OnLookupComplete;
        }

        /// <summary>
        /// ESA Parser On Lookup Complete
        /// </summary>
        /// <param name="lookUpValue">lookup value</param>
        private void EsaParser_OnLookupComplete(string lookUpValue)
        {
            this.RaiseEventForLookUpComplete(lookUpValue);
            Server.SendAcknowledgement(lookUpValue);
        }

        /// <summary>
        /// On Request Received
        /// </summary>
        /// <param name="incomingData">incoming data</param>
        private void SServer_OnRequestReceived(string incomingData)
        {
            this.RaiseEventForRequestReceived(incomingData);
            switch (incomingData)
            {
                case BaseController.PortNumber:
                case BaseController.DeviceName:
                    break;
                default:
                    this.parser.FetchTestLookUpValue(incomingData);
                    break;
            }
        }
    }
}

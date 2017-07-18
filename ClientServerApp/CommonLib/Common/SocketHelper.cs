using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace CommonLib.Common
{
    internal class SocketHelper
    {
        #region Constants

        /// <summary>
        /// Starting Port Number
        /// </summary>
        private const int StartingPortNumber = 12345;
        #endregion

        /// <summary>
        /// Gets Starting Port
        /// </summary>
        internal static int StartingPort
        {
            get
            {
                return StartingPortNumber;
            }
        }

        /// <summary>
        /// Checks for Port Availability
        /// </summary>
        /// <param name="portNo">Accepts Param Name</param>
        /// <returns>boolean value</returns>
        internal static bool IsPortAvailable(int portNo)
        {
            IPGlobalProperties ipGlobalProperties = IPGlobalProperties.GetIPGlobalProperties();
            IPEndPoint[] tcpConnInfoArray = ipGlobalProperties.GetActiveTcpListeners();

            foreach (var endpoint in tcpConnInfoArray)
            {
                if (endpoint.Port == portNo)
                {
                    return true;
                }
            }

            return false;
        }
    }
}

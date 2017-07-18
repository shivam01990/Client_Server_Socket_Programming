using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLib.Adapter.Communication.Interface
{
    public interface ISocketAdapter
    {
        /// <summary>
        /// Connect method
        /// </summary>
        void Connect();

        /// <summary>
        /// Send Data 
        /// </summary>
        /// <param name="content">Content string</param>
        void SendData(string content);

        /// <summary>
        /// Close method
        /// </summary>
        void Close();
    }
}

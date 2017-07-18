using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLib.Parser
{
    internal abstract class BaseParser
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BaseParser" /> class.
        /// </summary>
        protected BaseParser()
        {
        }

        /// <summary>
        /// LookUp Complete Delegate
        /// </summary>
        /// <param name="lookUpValue">lookup value</param>
        internal delegate void LookUpCompleteDelegate(string lookUpValue);

        /// <summary>
        /// OnLookup Complete
        /// </summary>
        internal event LookUpCompleteDelegate OnLookupComplete;

        /// <summary>
        /// Fetch Test LookUp Value
        /// </summary>
        /// <param name="data">data string</param>
        public virtual void FetchTestLookUpValue(string data)
        {
        }

        /// <summary>
        /// Raise On Lookup Complete
        /// </summary>
        /// <param name="value">value string</param>
        internal void RaiseOnLookupComplete(string value)
        {
            this.OnLookupComplete?.Invoke(value);
        }
    }
}


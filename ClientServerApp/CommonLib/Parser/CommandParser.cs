using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace CommonLib.Parser
{
    internal class CommandParser : BaseParser
    {
        #region Varables
        /// <summary>
        /// Node not found
        /// </summary>
        private const string NodeNotFound = "<Command Not Found>";

        /// <summary>
        /// xml file path
        /// </summary>
        private readonly string xmlFilePath = string.Empty;

        /// <summary>
        /// Xml Content
        /// </summary>
        private string xmlContent = string.Empty;

        /// <summary>
        /// XElement element
        /// </summary>
        private IEnumerable<XElement> xElements = null;
        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandParser" /> class.
        /// </summary>
        /// <param name="xmlfilePath">xml file path</param>
        public CommandParser()
        {
        }

        /// <summary>
        /// Fetch Test LookUp Value
        /// </summary>
        /// <param name="dataToLookUp">data To LookUp</param>
        public override void FetchTestLookUpValue(string dataToLookUp)
        {

            base.FetchTestLookUpValue(dataToLookUp);
            this.RaiseOnLookupComplete(dataToLookUp);
        }

    }
}

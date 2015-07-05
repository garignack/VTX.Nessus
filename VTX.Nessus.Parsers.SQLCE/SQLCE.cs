using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.Concurrent;
using System.Linq;
using System.Xml.Linq;
using System.Xml.Schema;
using System.Xml.XPath;
using System.Xml.Xsl;

using VTX.Nessus;

namespace VTX.Nessus.Parsers
{

    public class SQLCE : IParser
    {
        private NessusClientDataV2 _NessusFile;

        public void New (string ConnectString)
            {

            }
        public void Clear(string ConnectString)
        {

        }
        public void Parse(string FilePath, string ConnectString)
        {
            try
            {

            }
            if (FilePath == null) { throw new ArgumentNullException("Must provide a valid path to a file "); }
            _NessusFile = new NessusClientDataV2(FilePath);
            foreach (VTX.Nessus.NessusXML ReportHost in _NessusFile.ReportHosts)
            {
                
            }
        }

        private void setupMultiThreadEnvironment()
        {

        }
    }
}

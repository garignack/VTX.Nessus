using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Text;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;
using System.Xml.XPath;
using System.Xml.Xsl;
using VTX.Utilities;

namespace VTX.Nessus
{
    public class ReportHost
    {
        private bool _cache = false;
        private XElement _xml;

        public void Load()
        {
            _xml = this.parse();
        }

        private XElement parse()
        {
            FileUtilities fileUtility = new FileUtilities();
            string reportHostXMLString = fileUtility.GetFileString(FilePath, StartFileLocation, EndFileLocation);
            return XElement.Parse(reportHostXMLString);
        }

        public string Name { get; set; }
        public string FilePath { get; set; }
        public int StartFileLocation { get; set; }
        public int EndFileLocation { get; set; }

        public XElement XML
        {
            get
            {
                if (_xml == null)
                {
                    if (Cache)
                    {
                        _xml = this.parse();
                        return _xml;
                    }
                    else
                    {
                        return this.parse();
                    }
                }
                return _xml;
            }
            set
            {

            }
        }
        public bool Cache
        {
            get { return _cache; }
            set
            {
                if (value)
                {
                    _cache = true;
                }
                else
                {
                    _cache = false;
                    _xml = null;
                }
            }
        }
    };

}

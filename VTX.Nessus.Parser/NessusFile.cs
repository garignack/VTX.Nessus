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
    public class NessusClientDataV2
    {
        //+++++++ Constants

        //+++++++ Fields
        private bool _initialized;
        private bool _cache;
        private long _fileSize;
        private int _hostCount;
        private FileUtilities fileUtility;
        private ConcurrentDictionary<string, ReportHost> _hostDictionary;
        private string _filePath;
        private string _reportname;

        //+++++++ Constructors
        public NessusClientDataV2()
        {

        }

        public NessusClientDataV2(string filePath)
        {
            _filePath = filePath;

            Initialize();

        }
        //+++++++ Destructors

        //+++++++ Public Methods
        public IEnumerator<System.Collections.Generic.KeyValuePair<string, VTX.Nessus.ReportHost>> GetEnumerator()
        {
            return _hostDictionary.GetEnumerator();
        }

        //+++++++ Private Methods

        private void Initialize()
        {
            fileUtility = new FileUtilities();
            // Verify File exists and is a Nessus File 
            if (File.Exists(_filePath) == false) { throw new FileNotFoundException("File Not Found", _filePath); }
            _fileSize = fileUtility.GetFileLength(_filePath);
            if (_fileSize > int.MaxValue) { throw new ArgumentOutOfRangeException("File size exceeds maximum file size"); }
            if (fileUtility.FindStringFirstLocation("<NessusClientData_v2>", _filePath) == 0) { ThrowBadNessusFile("Not a valid Nessus_V2 file"); }

            // Find Locations of Key nodes within File
            int reportnamelocation = fileUtility.FindStringFirstLocation("<Report ", _filePath);
            if (reportnamelocation == 0) { ThrowBadNessusFile("No Scan Results Found"); }

            List<int> hostListLocations = fileUtility.FindStringLocations("<ReportHost ", _filePath);
            
            _hostCount = hostListLocations.Count();
            string reportNameNode = fileUtility.GetFileString(_filePath, reportnamelocation, reportnamelocation + 264);
            _reportname = reportNameNode.Substring(reportNameNode.IndexOf("name=\"") + 6, reportNameNode.IndexOf("\" ") - reportNameNode.IndexOf("name=\"") - 6);

            //Setup Concurrent Dictionary
//            int numProcs = Environment.ProcessorCount;
//            int concurrencyLevel = numProcs * 2;
            _hostDictionary = new ConcurrentDictionary<string, ReportHost>();

            for (int i = 0; i < _hostCount; i++)
                {
                int reportHostStartLocation = hostListLocations[i];
                int reportHostEndLocation = hostListLocations[i] + 1;
                if (i == _hostCount - 1)
                {
                    reportHostEndLocation = fileUtility.FindStringNextLocation("</Report>", _filePath, reportHostStartLocation);
                }
                else { reportHostEndLocation = hostListLocations[i + 1]; }


                string reportHostNode = fileUtility.GetFileString(_filePath, reportHostStartLocation, reportHostStartLocation + 264);


                ReportHost reportHost = new ReportHost();
                string reportHostName = reportHostNode.Substring(reportHostNode.IndexOf("name=\"") + 6, reportHostNode.IndexOf("\">") - reportHostNode.IndexOf("name=\"") - 6);
                reportHost.Name = reportHostName;
                reportHost.FilePath = _filePath;
                reportHost.StartFileLocation = reportHostStartLocation;
                reportHost.EndFileLocation = reportHostEndLocation;
                if (!( _hostDictionary.TryAdd(reportHost.Name, reportHost)))
                {
                    int j = 1;
                    do
                    {
                        j++;
                        reportHost.Name = String.Format("{0}({1})", reportHost.Name, j);
                        if (j > _hostCount) { ThrowBadNessusFile(String.Format("Unable to process ReportHost entry {0}", reportHostName)); }
                    } while (!(_hostDictionary.TryAdd(reportHost.Name, reportHost)));

                }

            }

            _initialized = true;
        }


        private void ThrowBadNessusFile(string message)
        {
            throw new FormatException(String.Format("There was an error parsing {0}: {1}", _filePath, message));
        }
        
        //+++++++ Properties

        public bool Initialized
        {
            get { return _initialized; }
        }

        public bool Cache
        {
            get { return _cache; }
            set
            {
                if (value)
                {
                    foreach (ReportHost reportHost in _hostDictionary.Values)
                    {
                        reportHost.Cache = true;
                    }
                    _cache = true;
                }
                else
                {
                    foreach (ReportHost reportHost in _hostDictionary.Values)
                    {
                        reportHost.Cache = false;
                    }
                    _cache = false;
                }
            }
        }

        public string FilePath
        {
            get { return _filePath; }
            set
            {

            }
        }

        public string ReportName
        {
            get { return _reportname; }
            set { }
        }
        public System.Collections.Generic.ICollection<VTX.Nessus.ReportHost> ReportHosts
        {
            get { return _hostDictionary.Values; }
            set { }
        }

        public System.Collections.Generic.ICollection<string> ReportHostList
        {
            get { return _hostDictionary.Keys; }
            set { }
        }
    }
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
            get {
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
            set {

            }
        }
        public bool Cache
        {
            get { return _cache; }
            set {
                if(value)
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

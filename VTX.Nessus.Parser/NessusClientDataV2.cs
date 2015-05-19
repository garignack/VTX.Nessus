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
        private bool _cache;
        private FileUtilities fileUtility;
        private ConcurrentDictionary<string, NessusXML> _hostDictionary;

        //+++++++ Constructors
        public NessusClientDataV2()
        {

        }

        public NessusClientDataV2(string filePath)
        {
            FilePath = filePath;

            Initialize();

        }
        //+++++++ Destructors

        //+++++++ Public Methods
        public IEnumerator<System.Collections.Generic.KeyValuePair<string, VTX.Nessus.NessusXML>> GetEnumerator()
        {
            return _hostDictionary.GetEnumerator();
        }

        //+++++++ Private Methods

        private void Initialize()
        {
            fileUtility = new FileUtilities();
            // Verify File exists and is a Nessus File 
            if (File.Exists(FilePath) == false) { throw new FileNotFoundException("File Not Found", FilePath); }
            FileSize = fileUtility.GetFileLength(FilePath);
            if (FileSize > int.MaxValue) { throw new ArgumentOutOfRangeException("File size exceeds maximum file size"); }
            if (fileUtility.FindStringFirstLocation("<NessusClientData_v2>", FilePath) == 0) { ThrowBadNessusFile("Not a valid Nessus_V2 file"); }

            // Find Locations of Key nodes within File
            int reportnamelocation = fileUtility.FindStringFirstLocation("<Report ", FilePath);
            if (reportnamelocation == 0) { ThrowBadNessusFile("No Scan Results Found"); }

            List<int> hostListLocations = fileUtility.FindStringLocations("<ReportHost ", FilePath);
            
            HostCount = hostListLocations.Count();
            string reportNameNode = fileUtility.GetFileString(FilePath, reportnamelocation, reportnamelocation + 264);
            ReportName = reportNameNode.Substring(reportNameNode.IndexOf("name=\"") + 6, reportNameNode.IndexOf("\" ") - reportNameNode.IndexOf("name=\"") - 6);

            //Setup Concurrent Dictionary
//            int numProcs = Environment.ProcessorCount;
//            int concurrencyLevel = numProcs * 2;
            _hostDictionary = new ConcurrentDictionary<string, NessusXML>();

            for (int i = 0; i < HostCount; i++)
                {
                int reportHostStartLocation = hostListLocations[i];
                int reportHostEndLocation = hostListLocations[i] + 1;
                if (i == HostCount - 1)
                {
                    reportHostEndLocation = fileUtility.FindStringNextLocation("</Report>", FilePath, reportHostStartLocation);
                }
                else { reportHostEndLocation = hostListLocations[i + 1]; }


                string reportHostNode = fileUtility.GetFileString(FilePath, reportHostStartLocation, reportHostStartLocation + 264);


                NessusXML reportHost = new NessusXML();
                string reportHostName = reportHostNode.Substring(reportHostNode.IndexOf("name=\"") + 6, reportHostNode.IndexOf("\">") - reportHostNode.IndexOf("name=\"") - 6);
                reportHost.Name = reportHostName;
                reportHost.FilePath = FilePath;
                reportHost.FileStartLocation = reportHostStartLocation;
                reportHost.FileEndLocation = reportHostEndLocation;
                if (!( _hostDictionary.TryAdd(reportHost.Name, reportHost)))
                {
                    int j = 1;
                    do
                    {
                        j++;
                        reportHost.Name = String.Format("{0}({1})", reportHost.Name, j);
                        if (j > HostCount) { ThrowBadNessusFile(String.Format("Unable to process ReportHost entry {0}", reportHostName)); }
                    } while (!(_hostDictionary.TryAdd(reportHost.Name, reportHost)));

                }

            }

            Initialized = true;
        }


        private void ThrowBadNessusFile(string message)
        {
            throw new FormatException(String.Format("There was an error parsing {0}: {1}", FilePath, message));
        }
        
        //+++++++ Properties

        public bool Cache
        {
            get { return _cache; }
            set
            {
                if (value)
                {
                    foreach (NessusXML reportHost in _hostDictionary.Values)
                    {
                        reportHost.Cache = true;
                    }
                    _cache = true;
                }
                else
                {
                    foreach (NessusXML reportHost in _hostDictionary.Values)
                    {
                        reportHost.Cache = false;
                    }
                    _cache = false;
                }
            }
        }

        public string FilePath { get; private set; }

        public string ReportName { get; private set; }

        public long FileSize { get; private set; }

        public bool Initialized { get; private set; }

        public int HostCount { get; private set; }

        public System.Collections.Generic.ICollection<VTX.Nessus.NessusXML> ReportHosts
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
}

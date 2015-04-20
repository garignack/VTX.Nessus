using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Text;
using System.IO;
using System.Linq;
using VTX.Utilities;

namespace VTX.Nessus
{
    public class NessusClientDataV2
    {
        //+++++++ Constants

        //+++++++ Fields
        private bool _initialized;
        private int fileSize;
        private int hostCount;
        private FileUtilities fileUtility;
        private ConcurrentDictionary<string, int> hostList;
        private string _filePath;

        //+++++++ Constructors
        public NessusClientDataV2() 
        {

        }

        public NessusClientDataV2(string filePath)
        {
            _filePath = filePath;
            this.Initialize();

        }

        private void Initialize()
        {
            fileUtility = new FileUtilities();
            if (File.Exists(_filePath) == false) { throw new FileNotFoundException("File Not Found", _filePath); }
            if (this.fileUtility.FindStringFirstLocation("<NessusClientData_v2>", _filePath) == 0) { throw new FormatException("Not a valid Nessus_V2 file"); }
            
            
            _initialized = true;
        }

        //+++++++ Destructors


        //+++++++ Properties

        public string FilePath
        {
            get { return _filePath; }
            set
            {

            }
        }
    }
}

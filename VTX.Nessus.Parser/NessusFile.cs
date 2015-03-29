using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Text;
using System.IO;
using VTX.Utilities;

namespace VTX.Nessus
{
    class NessusClientDataV2
    {
        private int fileSize;
        private int hostCount;
        private FileUtilities fileUtility;
        private ConcurrentDictionary<string, int> hostList;
        private string filePath;

        public NessusClientDataV2() 
        {
            fileUtility = new FileUtilities();

        }

        public NessusClientDataV2(string filePath)
        {
            fileUtility = new FileUtilities();
            if (File.Exists(filePath) == false) { throw new FileNotFoundException("File Not Found", filePath); }
            if (fileUtility.FindStringFirstLocation("<NessusClientData_v2>", filePath) == 0) { throw new FormatException("Not a valid Nessus_V2 file"); }

        }
    }
}

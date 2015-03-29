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

        public NessusClientDataV2() 
        {
            fileUtility = new FileUtilities();

        }

        public NessusClientDataV2(string FILE_NAME)
        {
            fileUtility = new FileUtilities();
            if (File.Exists(filePath) == false) { throw new FileNotFoundException("File Not Found", FILE_NAME); }
            if (fileUtility.FindBytePatternFirstLocation(){ }
        }
    }
}

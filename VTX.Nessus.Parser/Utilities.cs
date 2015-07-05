using System;
using System.IO;
using System.Collections.Generic;
using System.Text;

namespace VTX.Utilities
{
    public class FileUtilities
    {

        /// <summary>
        /// Implements Boyd-Moyer-HorsePool Algorithm. Adapted from http://aspdotnetcodebook.blogspot.com/2013/04/boyer-moore-search-algorithm.html
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static int FindBytePatternNextLocation(byte[] pattern, string filePath, int startLocation, int bufferSize = 65536)
        {

            byte[] needle = pattern;
            if (needle.Length > bufferSize) { bufferSize = needle.Length * 2; }
            byte[] haystack = new byte[bufferSize];

            Int32 match = new Int32();
            using (FileStream r = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {

                int numBytesToRead = (int)r.Length;
                if (startLocation > numBytesToRead) { startLocation = 0; }
                if (needle.Length > numBytesToRead)
                {
                    return match;
                }
                int[] badShift = BuildBadCharTable(needle);

                r.Seek(startLocation, SeekOrigin.Begin);
                while (numBytesToRead > 0)
                {
                    int pos = (int)r.Position;
                    int n = r.Read(haystack, 0, bufferSize);
                    if (n == 0) { break; }
                    while (needle.Length > n)
                    {
                        byte[] buffer = new byte[bufferSize - n];
                        int o = r.Read(buffer, 0, buffer.Length);
                        if (o == 0) { break; }
                        haystack.CopyTo(buffer, n);
                        n = n + o;
                    }
                    numBytesToRead = numBytesToRead - n;
                    int offset = 0;
                    int scan = 0;
                    int last = needle.Length - 1;
                    int maxoffset = haystack.Length - needle.Length;
                    while (offset <= maxoffset)
                    {
                        for (scan = last; (needle[scan] == haystack[scan + offset]); scan--)
                        {
                            if (scan == 0)
                            { //Match found
                                match = pos + offset;
                                return match;
                            }
                        }
                        if (offset + last > haystack.Length - 1) { break; }
                        offset += badShift[(int)haystack[offset + last]];
                    }
                    r.Position = pos + n - needle.Length;
                }
            }
            return match;
        }
        /// <summary>
        /// Implements Boyd-Moyer-HorsePool Algorithm. Adapted from http://aspdotnetcodebook.blogspot.com/2013/04/boyer-moore-search-algorithm.html
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static List<int> FindBytePatternLocations(byte[] pattern, string filePath, int bufferSize = 65536)
        {


            byte[] needle = pattern;
            if (needle.Length > bufferSize) { bufferSize = needle.Length * 2; }
            byte[] haystack = new byte[bufferSize];

            List<int> matches = new List<int>();
            using (FileStream r = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                int numBytesToRead = (int)r.Length;
                if (needle.Length > numBytesToRead)
                {
                    return matches;
                }
                int[] badShift = BuildBadCharTable(needle);

                while (numBytesToRead > 0)
                {
                    int pos = (int)r.Position;
                    int n = r.Read(haystack, 0, bufferSize);
                    if (n == 0) { break; }
                    while (needle.Length > n)
                    {
                        byte[] buffer = new byte[bufferSize - n];
                        int o = r.Read(buffer, 0, buffer.Length);
                        if (o == 0) { break; }
                        haystack.CopyTo(buffer, n);
                        n = n + o;
                    }
                    numBytesToRead = numBytesToRead - n;
                    int offset = 0;
                    int scan = 0;
                    int last = needle.Length - 1;
                    int maxoffset = haystack.Length - needle.Length;
                    while (offset <= maxoffset)
                    {
                        for (scan = last; (needle[scan] == haystack[scan + offset]); scan--)
                        {
                            if (scan == 0)
                            { //Match found
                                int i = pos + offset;
                                matches.Add(i);
                                offset++;
                                break;
                            }
                        }
                        if (offset + last > haystack.Length - 1) { break; }
                        offset += badShift[(int)haystack[offset + last]];
                    }
                    r.Position = pos + n - needle.Length;
                }
            }
            return matches;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public int FindBytePatternFirstLocation(byte[] pattern, string filePath, int bufferSize = 65536)
        {
            return FindBytePatternNextLocation(pattern, filePath, 0, bufferSize);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public int FindStringFirstLocation (string searchString, string filePath, int bufferSize = 65536)
        {

            byte[] pattern = GetEncoding(filePath).GetBytes(searchString);
            return FindBytePatternNextLocation(pattern, filePath, 0, bufferSize);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public int FindStringNextLocation(string searchString, string filePath, int startLocation, int bufferSize = 65536)
        {

            byte[] pattern = GetEncoding(filePath).GetBytes(searchString);
            return FindBytePatternNextLocation(pattern, filePath, startLocation, bufferSize);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public List<int> FindStringLocations(string searchString, string filePath, int bufferSize = 65536)
        {

            byte[] pattern = GetEncoding(filePath).GetBytes(searchString);
            return FindBytePatternLocations(pattern, filePath, bufferSize);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns> 
        private static int[] BuildBadCharTable(byte[] needle)
        {
            int[] badShift = new int[256];
            for (int i = 0; i < 256; i++)
            {
                badShift[i] = needle.Length;
            }
            int last = needle.Length - 1;
            for (int i = 0; i < last; i++)
            {
                badShift[(int)needle[i]] = last - i;
            }
            return badShift;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public byte[] GetFileBytes(string filePath, int StartLocation, int EndLocation)
        {
            // Gets the bytes of a file between the StartLocation to EndLocation
            byte[] buffer;
            using (FileStream r = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                int maxSize = (int)r.Length;
                if (EndLocation == -1 || EndLocation > maxSize) { EndLocation = maxSize; }
                if (StartLocation < 0) { StartLocation = 0; }

                int length = EndLocation - StartLocation;
                buffer = new byte[length];
                r.Seek(StartLocation, SeekOrigin.Begin);
                r.Read(buffer, 0, length);
            }
            return buffer;

        }

        public string GetFileString(string filePath, int StartLocation, int EndLocation)
        {
            // Gets the bytes of a file between the StartLocation to EndLocation
            byte[] buffer;
            using (FileStream r = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                int maxSize = (int)r.Length;
                if (EndLocation == -1 || EndLocation > maxSize) { EndLocation = maxSize; }
                if (StartLocation < 0) { StartLocation = 0; }

                int length = EndLocation - StartLocation;
                buffer = new byte[length];
                r.Seek(StartLocation, SeekOrigin.Begin);
                r.Read(buffer, 0, length);
            }
            return new string(GetEncoding(filePath).GetChars(buffer));

        }

        /// <summary>
        /// Determines a text file's encoding by analyzing its byte order mark (BOM).
        /// Defaults to ASCII when detection of the text file's endianness fails.
        /// </summary>
        /// <param name="filePath">The text file to analyze.</param>
        /// <returns>The detected encoding.</returns>
        public static Encoding GetEncoding(string filePath)
        {
            // Read the BOM
            var bom = new byte[4];
            using (var file = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                file.Read(bom, 0, 4);
            }

            // Analyze the BOM
            if (bom[0] == 0x2b && bom[1] == 0x2f && bom[2] == 0x76) return Encoding.UTF7;
            if (bom[0] == 0xef && bom[1] == 0xbb && bom[2] == 0xbf) return Encoding.UTF8;
            if (bom[0] == 0xff && bom[1] == 0xfe) return Encoding.Unicode; //UTF-16LE
            if (bom[0] == 0xfe && bom[1] == 0xff) return Encoding.BigEndianUnicode; //UTF-16BE
            if (bom[0] == 0 && bom[1] == 0 && bom[2] == 0xfe && bom[3] == 0xff) return Encoding.UTF32;
            return Encoding.Default;
        }

        public long GetFileLength(string filePath) {
            System.IO.FileInfo fileInfo = new System.IO.FileInfo(filePath);
            long length = fileInfo.Length;
            return length;
        }

    }
}

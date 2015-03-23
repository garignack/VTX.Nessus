using System;
using System.IO;
using System.Collections.Generic;
using System.Text;

namespace VTX.Utilities
{
    public class File
    {
        public static List<int> FindBytePatternOffset(byte[] pattern, string FILE_NAME, int bufferSize = 65536)
        {
            //Implements Boyd-Moyer-HorsePool Algorithm
            //Adapted from http://aspdotnetcodebook.blogspot.com/2013/04/boyer-moore-search-algorithm.html

            byte[] needle = pattern;
            if (needle.Length > bufferSize) { bufferSize = needle.Length * 2; }
            byte[] haystack = new byte[bufferSize];

            List<int> matches = new List<int>();
            using (FileStream r = new FileStream(FILE_NAME, FileMode.Open, FileAccess.Read))
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

        public static int[] BuildBadCharTable(byte[] needle)
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

        public static byte[] GetFileBytes(string FileName, int StartLocation, int EndLocation)
        {
            // Gets the bytes of a file between the StartLocation to EndLocation
            byte[] buffer;
            using (FileStream r = new FileStream(FileName, FileMode.Open, FileAccess.Read))
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

    }
}

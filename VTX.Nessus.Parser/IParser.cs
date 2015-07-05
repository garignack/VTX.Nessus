using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VTX.Nessus
{
    public interface IParser
    {
        void New(string ConnectString);

        void Clear(string ConnectString);

        void Parse(string FilePath, string ConnectString);

    }
}

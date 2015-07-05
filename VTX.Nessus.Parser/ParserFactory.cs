using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.IO;


namespace VTX.Nessus
{
    public class ParserFactory
    {

        #region Member Variables

        // Directory that contains Parser Libraries
        private static string ParserDir;


        // reference to the ILogger object.  Get a reference the first time then keep it
        private static IParser Parser;

        // This variable is used as a lock for thread safety
        private static object lockObject = new object();

        #endregion


        public static IParser GetParser(string ParserName)
        {
            lock (lockObject)
            {
                if (Parser == null)
                {
                    string asm_name = ParserDir + ParserName;
                    string class_name = ParserName;

                    if (String.IsNullOrEmpty(asm_name) || String.IsNullOrEmpty(class_name))
                        throw new ArgumentNullException("Missing Parser Name");
                    if (File.Exists(asm_name) == false) { throw new FileNotFoundException("Parser Not Found", class_name); }

                    Assembly assembly = Assembly.LoadFrom(asm_name);
                    Parser = assembly.CreateInstance(class_name) as IParser;

                    if (Parser == null)
                        throw new EntryPointNotFoundException(
                            string.Format("Unable to instantiate IParser class {0}/{1}",
                            asm_name, class_name));
                }
                return Parser;
            }
        }
    }
}

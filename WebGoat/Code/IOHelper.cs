using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;

namespace OWASP.WebGoat.NET
{
    public class IOHelper
    {
        public static string ReadAllFromFile(string path)
        {
            var normalizedPath = Path.GetFullPath(path);
            string safeBase = "C:\\app\\safeDirectory"; // TODO: Replace the placeholder with the actual safe directory path
            if (!normalizedPath.StartsWith(safeBase, StringComparison.OrdinalIgnoreCase))
            {
                throw new ArgumentException("Invalid file path.");
           
            }
            FileStream fs = new FileStream(normalizedPath, FileMode.OpenOrCreate, FileAccess.Read);
            StreamReader sr = new StreamReader(fs);
            string data = sr.ReadToEnd();
            sr.Close();
            return data;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Web;
using System.IO;

namespace OWASP.WebGoat.NET
{
    public class IOHelper
    {
        public static string ReadAllFromFile(string path)
        {
            // Validate the input path to prevent path traversal
            if (path.Contains("..") || Path.IsPathRooted(path)) {
                throw new ArgumentException("Invalid file path");
            }
            FileStream fs = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Read);
            StreamReader sr = new StreamReader(fs);
            string data = sr.ReadToEnd();
            sr.Close();
            return data;
        }
    }
}

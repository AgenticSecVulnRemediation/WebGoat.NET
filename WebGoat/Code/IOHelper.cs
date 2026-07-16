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
                        // Validate the input path to prevent directory traversal and absolute paths
            if (path.Contains("..") || System.IO.Path.IsPathRooted(path)) {
                throw new System.ArgumentException("Invalid file path"); // TODO: Customize exception handling as required
            }
            FileStream fs = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Read);
            StreamReader sr = new StreamReader(fs);
            string data = sr.ReadToEnd();
            sr.Close();
            return data;
        }
    }
}

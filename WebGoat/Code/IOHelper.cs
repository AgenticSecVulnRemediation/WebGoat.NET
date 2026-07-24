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
            // Validate the file path input to prevent directory traversal
            if (Path.IsPathRooted(path) || path.Contains("..")) {
                // TODO: Adjust the exception message or handling as needed
                throw new ArgumentException("Invalid file path provided.", nameof(path));
            }
            FileStream fs = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Read);
            StreamReader sr = new StreamReader(fs);
            string data = sr.ReadToEnd();
            sr.Close();
            return data;
        }
    }
}

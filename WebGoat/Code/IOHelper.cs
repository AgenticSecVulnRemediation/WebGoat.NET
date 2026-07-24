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
        {            // Validate file path to prevent path traversal
            if (string.IsNullOrEmpty(path) || path.Contains("..") || Path.IsPathRooted(path))
            {
                // TODO: Replace the below exception message with an appropriate error handling mechanism if needed
                throw new ArgumentException("Invalid file path provided.");
            }
            using (FileStream fs = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Read))
            using (StreamReader sr = new StreamReader(fs))
                {
                    return sr.ReadToEnd();
                }
        }
    }
}

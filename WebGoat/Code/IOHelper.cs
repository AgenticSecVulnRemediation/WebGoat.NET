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
                        // Normalize the path and validate against a safe base directory
            string normalizedPath = Path.GetFullPath(path);
            // Replace 'YOUR_SAFE_BASE_DIRECTORY' with the absolute path to the approved directory
            string safeBaseDir = Path.GetFullPath("YOUR_SAFE_BASE_DIRECTORY");
            if (!normalizedPath.StartsWith(safeBaseDir, StringComparison.OrdinalIgnoreCase)) {
                throw new UnauthorizedAccessException("Access Denied: Invalid file path.");
            }
            FileStream fs = new FileStream(normalizedPath, FileMode.OpenOrCreate, FileAccess.Read);
            StreamReader sr = new StreamReader(fs);
            string data = sr.ReadToEnd();
            sr.Close();
            return data;
        }
    }
}

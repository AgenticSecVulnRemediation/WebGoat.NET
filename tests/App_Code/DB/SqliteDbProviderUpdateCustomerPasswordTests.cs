using System;
using System.IO;
using Xunit;

// Source-level regression test for PR 3965: UpdateCustomerPassword uses parameterized SQL.

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderUpdateCustomerPasswordTests
    {
        [Fact]
        public void UpdateCustomerPassword_UsesParameters_ForPasswordAndCustomerNumber()
        {
            var code = ReadRepoFile("WebGoat", "App_Code", "DB", "SqliteDbProvider.cs");

            Assert.Contains("UPDATE CustomerLogin SET password = @Password WHERE customerNumber = @CustomerNumber", code);
            Assert.Contains("command.Parameters.AddWithValue(\"@Password\"", code);
            Assert.Contains("command.Parameters.AddWithValue(\"@CustomerNumber\"", code);

            // Previously vulnerable concatenation patterns
            Assert.DoesNotContain("update CustomerLogin set password = '" + "\" + Encoder.Encode(password) + \"" + "'", code);
            Assert.DoesNotContain("where customerNumber = " + "\" + customerNumber", code);
        }

        private static string ReadRepoFile(params string[] parts)
        {
            string TryReadFrom(string root)
            {
                var p = Path.Combine(root, Path.Combine(parts));
                return File.Exists(p) ? File.ReadAllText(p) : null;
            }

            var cwd = Directory.GetCurrentDirectory();
            var fromCwd = TryReadFrom(cwd);
            if (fromCwd != null) return fromCwd;

            var dir = new DirectoryInfo(AppContext.BaseDirectory);
            for (int i = 0; i < 10 && dir != null; i++, dir = dir.Parent)
            {
                var candidate = TryReadFrom(dir.FullName);
                if (candidate != null) return candidate;
            }

            throw new FileNotFoundException($"Could not locate patched file: {Path.Combine(parts)} from cwd '{cwd}' or base '{AppContext.BaseDirectory}'.");
        }
    }
}

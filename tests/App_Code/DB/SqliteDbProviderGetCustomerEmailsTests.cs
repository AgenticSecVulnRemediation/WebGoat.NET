using System;
using System.IO;
using Xunit;

// Source-level regression test for PR 3972: GetCustomerEmails uses LIKE parameter placeholder.

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderGetCustomerEmailsTests
    {
        [Fact]
        public void GetCustomerEmails_UsesLikeParameterPlaceholder()
        {
            var code = ReadRepoFile("WebGoat", "App_Code", "DB", "SqliteDbProvider.cs");

            Assert.Contains("select email from CustomerLogin where email like @emailPattern", code);
            Assert.DoesNotContain("select email from CustomerLogin where email like '" + "\" + email + \"" + "%'", code);
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

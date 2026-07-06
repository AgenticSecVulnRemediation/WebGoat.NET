using System;
using System.IO;
using Xunit;

// Source-level regression test for PR 3961: GetPasswordByEmail now parameterizes email.

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderGetPasswordByEmailTests
    {
        [Fact]
        public void GetPasswordByEmail_UsesParameterizedEmailQuery()
        {
            var code = ReadRepoFile("WebGoat", "App_Code", "DB", "SqliteDbProvider.cs");

            Assert.Contains("select * from CustomerLogin where email = @email;", code);
            Assert.Contains("cmd.Parameters.AddWithValue(\"@email\", email", code);
            Assert.Contains("new SqliteDataAdapter(cmd)", code);

            Assert.DoesNotContain("select * from CustomerLogin where email = '" + "\" + email + \"" + "';", code);
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

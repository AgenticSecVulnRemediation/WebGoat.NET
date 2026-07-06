using System;
using System.IO;
using Xunit;

// Source-level regression test for PR 3945.
// Reads the patched file from the repository checkout and asserts that CustomCustomerLogin
// uses a parameter placeholder and binds it (no string concatenation).

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderCustomCustomerLoginTests
    {
        [Fact]
        public void CustomCustomerLogin_EmailQuery_IsParameterized_AndBindsEmail()
        {
            var code = ReadRepoFile("WebGoat", "App_Code", "DB", "SqliteDbProvider.cs");

            Assert.Contains("select * from CustomerLogin where email = @Email;", code);
            Assert.Contains("cmd.Parameters.AddWithValue(\"@Email\", email", code);
            Assert.Contains("new SqliteDataAdapter(cmd)", code);

            // Previously vulnerable concatenation patterns
            Assert.DoesNotContain("where email = '" + "\" + email + \"" + "'", code);
            Assert.DoesNotContain("where email = '" + "\" + email + \"" + "';", code);
        }

        private static string ReadRepoFile(params string[] parts)
        {
            // Prefer repo-root as current directory in CI; fall back to walking up from BaseDirectory.
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

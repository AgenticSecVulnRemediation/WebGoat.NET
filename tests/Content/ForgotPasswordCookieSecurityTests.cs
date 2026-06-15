using System;
using System.IO;
using Xunit;

namespace OWASP.WebGoat.NET.Tests
{
    public class ForgotPasswordCookieSecurityTests
    {
        [Fact]
        public void ForgotPassword_SourceSetsRecoveryCookie_HttpOnlyTrue()
        {
            // Delta behavior: PR sets cookie.HttpOnly = true for the security-answer cookie.
            // To keep this deterministic without building a full WebForms HttpContext,
            // we assert the fixed source code contains the expected assignment.

            var projectRoot = Directory.GetCurrentDirectory();

            // Tests are placed under /tests; the WebForms file is under /WebGoat/Content.
            // Traverse up from test working directory to find the repo root.
            // (This is robust across typical test runners.)
            string? dir = projectRoot;
            string? filePath = null;
            for (var i = 0; i < 8 && dir != null; i++)
            {
                var candidate = Path.Combine(dir, "WebGoat", "Content", "ForgotPassword.aspx.cs");
                if (File.Exists(candidate))
                {
                    filePath = candidate;
                    break;
                }
                dir = Directory.GetParent(dir)?.FullName;
            }

            Assert.False(string.IsNullOrEmpty(filePath), "Could not locate WebGoat/Content/ForgotPassword.aspx.cs from test working directory");

            var source = File.ReadAllText(filePath!);

            Assert.Contains("cookie.HttpOnly = true", source);
        }
    }
}

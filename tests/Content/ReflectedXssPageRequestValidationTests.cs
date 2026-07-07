using System;
using System.IO;
using Xunit;

namespace OWASP.WebGoat.NET.Tests
{
    public class ReflectedXssPageRequestValidationTests
    {
        [Fact]
        public void ReflectedXssAspx_EnablesValidateRequest()
        {
            // Arrange
            // Patch flips validateRequest from false to true.
            var path = Path.Combine(AppContext.BaseDirectory, "WebGoat", "Content", "ReflectedXSS.aspx");

            // The file may not be copied to test output in all setups; if so, skip deterministically.
            if (!File.Exists(path))
            {
                // Fallback: locate from repository relative path if tests run from repo root.
                var alt = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "WebGoat", "Content", "ReflectedXSS.aspx"));
                if (!File.Exists(alt))
                {
                    throw new FileNotFoundException("Could not locate ReflectedXSS.aspx for validation test.", path);
                }
                path = alt;
            }

            var content = File.ReadAllText(path);

            // Assert
            Assert.Contains("validateRequest=\"true\"", content);
            Assert.DoesNotContain("validateRequest=\"false\"", content);
        }
    }
}

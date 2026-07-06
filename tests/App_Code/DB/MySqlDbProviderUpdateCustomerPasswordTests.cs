using System;
using Moq;
using Xunit;

// Assumptions:
// - Source class is OWASP.WebGoat.NET.App_Code.DB.MySqlDbProvider
// - The provider uses MySql.Data.MySqlClient internally; we validate the *patched SQL shape* via source regression.
// This avoids DB/network and stays focused on the security fix (SQL parameterization).

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderUpdateCustomerPasswordTests
    {
        [Fact]
        public void UpdateCustomerPassword_UsesParameterizedQuery_DoesNotConcatenateCustomerNumberOrPassword()
        {
            // Arrange
            // We validate the post-fix command text & parameters by asserting the expected SQL/parameters are present
            // in the fixed file content. This is a deterministic delta test for the patch.
            var fileText = EmbeddedSourceReader.Read("WebGoat/App_Code/DB/MySqlDbProvider.cs");

            // Assert
            Assert.Contains("UPDATE CustomerLogin SET password = @password WHERE customerNumber = @customerNumber", fileText);
            Assert.Contains("Parameters.AddWithValue(\"@password\"", fileText);
            Assert.Contains("Parameters.AddWithValue(\"@customerNumber\"", fileText);

            // Previously vulnerable pattern (string concatenation in SQL) should not remain for this method.
            Assert.DoesNotContain("update CustomerLogin set password = '\" + Encoder.Encode(password) + \"' where customerNumber = \" + customerNumber", fileText);
        }
    }

    /// <summary>
    /// Minimal test helper to read repository files during unit tests.
    /// This is intentionally tiny and self-contained to avoid external dependencies.
    /// </summary>
    internal static class EmbeddedSourceReader
    {
        public static string Read(string relativePath)
        {
            // Try common base dirs for test execution.
            var candidates = new[]
            {
                AppContext.BaseDirectory,
                System.IO.Directory.GetCurrentDirectory()
            };

            foreach (var baseDir in candidates)
            {
                var path = System.IO.Path.GetFullPath(System.IO.Path.Combine(baseDir, "..", "..", "..", "..", relativePath));
                if (System.IO.File.Exists(path))
                    return System.IO.File.ReadAllText(path);

                path = System.IO.Path.GetFullPath(System.IO.Path.Combine(baseDir, "..", "..", "..", relativePath));
                if (System.IO.File.Exists(path))
                    return System.IO.File.ReadAllText(path);
            }

            throw new InvalidOperationException($"Could not locate source file: {relativePath}");
        }
    }
}

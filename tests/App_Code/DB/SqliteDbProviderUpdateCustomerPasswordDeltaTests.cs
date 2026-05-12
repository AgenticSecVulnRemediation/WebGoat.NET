using Xunit;
using System;

// Assumption: source namespace is OWASP.WebGoat.NET.App_Code.DB (per file content).
using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderUpdateCustomerPasswordDeltaTests
    {
        [Fact]
        public void UpdateCustomerPassword_ContainsParameterTokens_InAssembly()
        {
            // Arrange/Act
            // Delta check: fixed code introduces "@password" and "@customerNumber" tokens.
            // Avoids DB access and avoids constructing SqliteDbProvider (constructor requires ConfigFile).
            var asmPath = typeof(SqliteDbProvider).Assembly.Location;
            Assert.False(string.IsNullOrWhiteSpace(asmPath));

            var bytes = System.IO.File.ReadAllBytes(asmPath);

            // Assert
            Assert.True(ContainsSubsequence(bytes, System.Text.Encoding.UTF8.GetBytes("@password")));
            Assert.True(ContainsSubsequence(bytes, System.Text.Encoding.UTF8.GetBytes("@customerNumber")));
        }

        private static bool ContainsSubsequence(byte[] haystack, byte[] needle)
        {
            if (needle.Length == 0) return true;
            if (haystack.Length < needle.Length) return false;

            for (int i = 0; i <= haystack.Length - needle.Length; i++)
            {
                bool match = true;
                for (int j = 0; j < needle.Length; j++)
                {
                    if (haystack[i + j] != needle[j]) { match = false; break; }
                }
                if (match) return true;
            }
            return false;
        }
    }
}

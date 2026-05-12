using Xunit;
using System;

// Assumption: source namespace is OWASP.WebGoat.NET.App_Code.DB (per file content).
using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderGetEmailByCustomerNumberDeltaTests
    {
        [Fact]
        public void GetEmailByCustomerNumber_ContainsCustomerNumberParameterToken_InAssembly()
        {
            // Arrange/Act
            // Delta check: fixed code introduces parameter token "@CustomerNumber".
            var asmPath = typeof(MySqlDbProvider).Assembly.Location;
            Assert.False(string.IsNullOrWhiteSpace(asmPath));

            var bytes = System.IO.File.ReadAllBytes(asmPath);
            var tokenBytes = System.Text.Encoding.UTF8.GetBytes("@CustomerNumber");

            // Assert
            Assert.True(ContainsSubsequence(bytes, tokenBytes));
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

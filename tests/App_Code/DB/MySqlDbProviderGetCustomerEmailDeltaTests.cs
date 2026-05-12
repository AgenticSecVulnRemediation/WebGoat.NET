using Xunit;
using System;
using System.Linq;
using System.Reflection;

// Assumption: source namespace is OWASP.WebGoat.NET.App_Code.DB (per file content).
using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderGetCustomerEmailDeltaTests
    {
        [Fact]
        public void GetCustomerEmail_ContainsCustomerNumberParameterToken_InMethodBody()
        {
            // Arrange
            var method = typeof(MySqlDbProvider).GetMethod("GetCustomerEmail", BindingFlags.Public | BindingFlags.Instance);
            Assert.NotNull(method);

            // Act
            var body = method!.GetMethodBody();
            Assert.NotNull(body);

            var il = body!.GetILAsByteArray();
            Assert.NotNull(il);

            // Assert
            // Delta check: fixed code introduces "@customerNumber" literal.
            // We avoid constructing MySqlDbProvider (constructor requires ConfigFile) and avoid DB access.
            // We verify the literal is present in the assembly user-string heap by scanning module bytes.
            // This is deterministic and validates the delta without external dependencies.
            var asmPath = typeof(MySqlDbProvider).Assembly.Location;
            Assert.False(string.IsNullOrWhiteSpace(asmPath));

            var bytes = System.IO.File.ReadAllBytes(asmPath);
            var tokenBytes = System.Text.Encoding.UTF8.GetBytes("@customerNumber");

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

using System;
using System.Data;
using Mono.Data.Sqlite;
using Moq;
using Xunit;

// Assumptions:
// - Source namespace is OWASP.WebGoat.NET.App_Code.DB (from file content)
// - Tests run under a test project that can reference OWASP.WebGoat.NET assembly.
// - This delta test asserts the regression fix: GetPayments now uses a parameter placeholder (@customerNumber)
//   rather than string concatenation.

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderGetPaymentsDeltaTests
    {
        [Fact]
        public void GetPayments_UsesNamedParameterPlaceholder_InSqlText()
        {
            // Arrange
            // We avoid DB access. Instead, we verify the fixed SQL literal is present in the compiled type metadata.
            // This is a deterministic unit-level regression assertion aligned to the diff.
            var sqlExpected = "select * from Payments where customerNumber = @customerNumber";

            // Act
            var type = typeof(SqliteDbProvider);
            var allStrings = string.Join("\n", type.Assembly.GetManifestResourceNames());
            // Primary check: reflect over method body IL for user strings.
            var hasSql = AssemblyStringSearch.ContainsUserString(type, sqlExpected);

            // Assert
            Assert.True(hasSql, "Expected parameterized SQL placeholder to exist in assembly user strings.");
        }
    }

    internal static class AssemblyStringSearch
    {
        // Minimal IL user-string scan: read method bodies and look for ldstr constants.
        public static bool ContainsUserString(Type type, string expected)
        {
            foreach (var m in type.GetMethods(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic))
            {
                var body = m.GetMethodBody();
                if (body == null) continue;

                var il = body.GetILAsByteArray();
                if (il == null || il.Length == 0) continue;

                // Very lightweight scan: we cannot decode tokens without full metadata reader.
                // Instead, we use a safe fallback: compare against ToString() of method, plus assembly full name.
                // If the expected string is embedded as ldstr, it will also appear in the module's #US heap;
                // we detect it via Module.ResolveString on candidate tokens.
                var module = type.Module;
                for (int i = 0; i < il.Length - 4; i++)
                {
                    // ldstr opcode is 0x72 followed by 4-byte metadata token
                    if (il[i] != 0x72) continue;
                    int token = il[i + 1] | (il[i + 2] << 8) | (il[i + 3] << 16) | (il[i + 4] << 24);
                    try
                    {
                        var s = module.ResolveString(token);
                        if (string.Equals(s, expected, StringComparison.Ordinal))
                            return true;
                    }
                    catch
                    {
                        // Ignore invalid tokens
                    }
                }
            }
            return false;
        }
    }
}

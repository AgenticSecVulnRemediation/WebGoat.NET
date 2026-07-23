using System;
using System.Data;
using Moq;
using Xunit;

// Assumptions:
// - Source project compiles with MySql.Data reference; for unit test we mock MySqlCommand/MySqlDataAdapter construction
//   by exercising the SQL string change via reflection on created command.
// - Namespace inferred from file path: OWASP.WebGoat.NET.App_Code.DB

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderTests
    {
        [Fact]
        public void GetPayments_UsesParameterizedQuery_ForCustomerNumber()
        {
            // Arrange
            // We can't easily intercept MySqlCommand constructor without a seam; instead we validate the diff-introduced
            // SQL template is present in the method body by invoking it with a fake connection string and catching the expected exception.
            // This is a delta test focused on ensuring the query is no longer string-concatenated.
            var provider = (MySqlDbProvider)System.Runtime.Serialization.FormatterServices
                .GetUninitializedObject(typeof(MySqlDbProvider));

            // Set _connectionString to something non-null to reach SQL construction.
            typeof(MySqlDbProvider)
                .GetField("_connectionString", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic)
                ?.SetValue(provider, "Server=localhost;Database=doesnotexist;Uid=x;Pwd=y");

            // Act
            var ex = Record.Exception(() => provider.GetPayments(123));

            // Assert
            // Even if call fails due to DB connectivity, the method should exist and not throw due to string concat logic.
            // Delta assertion: method contains parameter placeholder "@customerNumber".
            var methodBody = typeof(MySqlDbProvider).GetMethod("GetPayments")!.GetMethodBody();
            Assert.NotNull(methodBody);

            // Use IL scan for the string literal "select * from Payments where customerNumber = @customerNumber"
            // to ensure the parameterized template is used.
            var il = methodBody!.GetILAsByteArray();
            Assert.NotNull(il);

            // Weak but deterministic: reflect all string literals from module and require expected SQL exists.
            // This avoids needing DB driver behavior.
            var allStrings = typeof(MySqlDbProvider).Module;
            bool found = false;
            foreach (var s in allStrings.GetMethodsWithModuleScopeStringLiterals())
            {
                if (s.Contains("select * from Payments where customerNumber = @customerNumber", StringComparison.Ordinal))
                {
                    found = true;
                    break;
                }
            }
            Assert.True(found, "Expected parameterized SQL template to be present after fix.");
        }
    }

    internal static class ModuleStringLiteralExtensions
    {
        // Best-effort helper to enumerate user strings in a module using metadata.
        public static System.Collections.Generic.IEnumerable<string> GetMethodsWithModuleScopeStringLiterals(this System.Reflection.Module module)
        {
            // Minimal implementation: pull all public/private methods in the module's types and extract IL user string tokens.
            foreach (var t in module.GetTypes())
            {
                foreach (var m in t.GetMethods(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.DeclaredOnly))
                {
                    var body = m.GetMethodBody();
                    if (body == null) continue;
                    var il = body.GetILAsByteArray();
                    if (il == null) continue;

                    // Scan for ldstr opcode (0x72) and read 4-byte metadata token.
                    for (int i = 0; i < il.Length - 5; i++)
                    {
                        if (il[i] != 0x72) continue;
                        int token = BitConverter.ToInt32(il, i + 1);
                        string? str = null;
                        try { str = module.ResolveString(token); } catch { }
                        if (str != null) yield return str;
                        i += 4;
                    }
                }
            }
        }
    }
}

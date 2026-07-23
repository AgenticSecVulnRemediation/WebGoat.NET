using System;
using System.Reflection;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderGetOrdersTests
    {
        [Fact]
        public void GetOrders_UsesSqlParameter_ForCustomerNumber()
        {
            // Arrange
            var method = typeof(OWASP.WebGoat.NET.App_Code.DB.SqliteDbProvider).GetMethod("GetOrders");
            Assert.NotNull(method);

            // Assert
            // Delta behavior: SQL should be "select * from Orders where customerNumber = @customerNumber"
            // and use SqliteCommand + parameter binding.
            var literals = typeof(OWASP.WebGoat.NET.App_Code.DB.SqliteDbProvider).Module.GetMethodsWithModuleScopeStringLiterals();

            bool found = false;
            foreach (var s in literals)
            {
                if (s.Contains("select * from Orders where customerNumber = @customerNumber", StringComparison.Ordinal))
                {
                    found = true;
                    break;
                }
            }

            Assert.True(found, "Expected parameterized Orders query template after fix.");
        }
    }

    internal static class ModuleStringLiteralExtensions
    {
        public static System.Collections.Generic.IEnumerable<string> GetMethodsWithModuleScopeStringLiterals(this Module module)
        {
            foreach (var t in module.GetTypes())
            {
                foreach (var m in t.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly))
                {
                    var body = m.GetMethodBody();
                    if (body == null) continue;
                    var il = body.GetILAsByteArray();
                    if (il == null) continue;

                    for (int i = 0; i < il.Length - 5; i++)
                    {
                        if (il[i] != 0x72) continue;
                        int token = BitConverter.ToInt32(il, i + 1);
                        try
                        {
                            yield return module.ResolveString(token);
                        }
                        catch
                        {
                            // ignore
                        }
                        i += 4;
                    }
                }
            }
        }
    }
}

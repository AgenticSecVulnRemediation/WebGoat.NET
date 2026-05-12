using System;
using Xunit;

// Assumptions:
// - Source namespace: OWASP.WebGoat.NET.App_Code.DB
// - Delta focus: UpdateCustomerPassword now uses parameter placeholders @password and @customerNumber.

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderUpdateCustomerPasswordDeltaTests
    {
        [Fact]
        public void UpdateCustomerPassword_UsesNamedParameters_InSqlText()
        {
            var type = typeof(OWASP.WebGoat.NET.App_Code.DB.MySqlDbProvider);
            Assert.True(AssemblyStringSearch.ContainsUserString(type, "update CustomerLogin set password = @password where customerNumber = @customerNumber"));
        }
    }

    internal static class AssemblyStringSearch
    {
        public static bool ContainsUserString(Type type, string expected)
        {
            foreach (var m in type.GetMethods(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic))
            {
                var body = m.GetMethodBody();
                if (body == null) continue;
                var il = body.GetILAsByteArray();
                if (il == null || il.Length == 0) continue;

                var module = type.Module;
                for (int i = 0; i < il.Length - 4; i++)
                {
                    if (il[i] != 0x72) continue;
                    int token = il[i + 1] | (il[i + 2] << 8) | (il[i + 3] << 16) | (il[i + 4] << 24);
                    try
                    {
                        var s = module.ResolveString(token);
                        if (string.Equals(s, expected, StringComparison.Ordinal))
                            return true;
                    }
                    catch { }
                }
            }
            return false;
        }
    }
}

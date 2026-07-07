using System;
using System.Reflection;
using Xunit;

// Delta test: verifies UpdateCustomerPassword uses parameters rather than concatenation.

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderUpdateCustomerPasswordParameterizedTests
    {
        [Fact]
        public void UpdateCustomerPassword_UsesParameterMarkers()
        {
            var module = typeof(MySqlDbProvider).Module;

            Assert.Contains("UPDATE CustomerLogin SET password = @password WHERE customerNumber = @customerNumber", GetAllUserStrings(module));
            Assert.DoesNotContain("update CustomerLogin set password = '", GetAllUserStrings(module));
        }

        private static string[] GetAllUserStrings(Module module)
        {
            var strings = new System.Collections.Generic.List<string>();
            for (int rid = 1; rid < 15000; rid++)
            {
                int token = unchecked((int)0x70000000) + rid;
                try
                {
                    var s = module.ResolveString(token);
                    if (!string.IsNullOrEmpty(s)) strings.Add(s);
                }
                catch { }
            }
            return strings.ToArray();
        }
    }
}

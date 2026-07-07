using System;
using System.Reflection;
using Xunit;

// Delta test: verifies GetOrders uses a parameter marker for customerNumber.

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderGetOrdersParameterizedTests
    {
        [Fact]
        public void GetOrders_UsesCustomerNumberParameterMarker()
        {
            var module = typeof(MySqlDbProvider).Module;
            Assert.Contains("select * from Orders where customerNumber = @customerNumber", GetAllUserStrings(module));
            Assert.DoesNotContain("select * from Orders where customerNumber = ", GetAllUserStrings(module));
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

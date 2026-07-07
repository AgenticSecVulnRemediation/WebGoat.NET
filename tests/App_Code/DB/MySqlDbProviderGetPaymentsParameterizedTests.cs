using System;
using System.Reflection;
using Xunit;

// Delta test: verifies GetPayments uses a parameter marker for customerNumber.

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderGetPaymentsParameterizedTests
    {
        [Fact]
        public void GetPayments_UsesCustomerNumberParameterMarker()
        {
            var module = typeof(MySqlDbProvider).Module;
            Assert.Contains("select * from Payments where customerNumber = @customerNumber", GetAllUserStrings(module));
            Assert.DoesNotContain("select * from Payments where customerNumber = ", GetAllUserStrings(module));
        }

        private static string[] GetAllUserStrings(Module module)
        {
            var strings = new System.Collections.Generic.List<string>();
            for (int rid = 1; rid < 10000; rid++)
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

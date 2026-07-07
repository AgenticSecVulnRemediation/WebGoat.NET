using System;
using System.Reflection;
using Xunit;

// Delta test: verifies MySqlHelper.ExecuteScalar call uses @num parameter placeholder.

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderGetEmailByCustomerNumberParameterizedTests
    {
        [Fact]
        public void GetEmailByCustomerNumber_UsesExecuteScalarWithParameter()
        {
            var module = typeof(MySqlDbProvider).Module;

            Assert.Contains("select email from CustomerLogin where customerNumber = @num", GetAllUserStrings(module));
            Assert.DoesNotContain("select email from CustomerLogin where customerNumber = ", GetAllUserStrings(module));
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

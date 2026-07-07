using System;
using System.Reflection;
using Xunit;

// Delta test: verifies GetPasswordByEmail uses a parameterized email predicate.

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderGetPasswordByEmailParameterizedTests
    {
        [Fact]
        public void GetPasswordByEmail_UsesEmailParameterMarker()
        {
            var module = typeof(MySqlDbProvider).Module;
            Assert.Contains("select * from CustomerLogin where email = @email;", GetAllUserStrings(module));
            Assert.DoesNotContain("select * from CustomerLogin where email = '", GetAllUserStrings(module));
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

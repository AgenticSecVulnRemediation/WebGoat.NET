using System;
using System.Reflection;
using Xunit;

// Delta test: verifies GetEmailByName uses parameterized LIKE and does not concatenate input.

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderGetEmailByNameParameterizedTests
    {
        [Fact]
        public void GetEmailByName_UsesNameParameterMarker()
        {
            var module = typeof(MySqlDbProvider).Module;
            Assert.Contains("select firstName, lastName, email from Employees where firstName like @name or lastName like @name", GetAllUserStrings(module));
            Assert.DoesNotContain("select firstName, lastName, email from Employees where firstName like '", GetAllUserStrings(module));
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

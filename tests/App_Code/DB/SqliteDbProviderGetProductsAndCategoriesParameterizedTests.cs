using System;
using System.Reflection;
using Xunit;

// Delta test: verifies GetProductsAndCategories uses parameterized catNumber queries when catNumber >= 1.

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderGetProductsAndCategoriesParameterizedTests
    {
        [Fact]
        public void GetProductsAndCategories_UsesCatNumberParameterMarker_InQueries()
        {
            var module = typeof(SqliteDbProvider).Module;

            Assert.Contains("select * from Categories where catNumber = @catNumber", GetAllUserStrings(module));
            Assert.Contains("select * from Products where catNumber = @catNumber", GetAllUserStrings(module));

            // Ensure old concatenated clause " where catNumber = " isn't present as a string literal.
            Assert.DoesNotContain(" where catNumber = ", GetAllUserStrings(module));
        }

        private static string[] GetAllUserStrings(Module module)
        {
            var strings = new System.Collections.Generic.List<string>();
            for (int rid = 1; rid < 20000; rid++)
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

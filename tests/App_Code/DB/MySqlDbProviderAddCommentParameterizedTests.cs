using System;
using System.Reflection;
using Xunit;

// Delta test: verifies AddComment no longer embeds raw values into SQL, and instead uses parameters.

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderAddCommentParameterizedTests
    {
        [Fact]
        public void AddComment_UsesParameterPlaceholders_InSqlString()
        {
            var module = typeof(MySqlDbProvider).Module;

            var expected = "insert into Comments(productCode, email, comment) values (@productCode, @Email, @Comment);";
            Assert.Contains(expected, GetAllUserStrings(module));

            // Ensure old vulnerable concatenated insert is not present.
            Assert.DoesNotContain("insert into Comments(productCode, email, comment) values ('", GetAllUserStrings(module));
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

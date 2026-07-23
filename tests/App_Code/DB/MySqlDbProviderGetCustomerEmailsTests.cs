using System;
using System.Reflection;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderGetCustomerEmailsTests
    {
        [Fact]
        public void GetCustomerEmails_UsesParameterMarker_InsteadOfStringConcatenation()
        {
            // Arrange/Act
            var method = typeof(MySqlDbProvider).GetMethod("GetCustomerEmails");
            Assert.NotNull(method);

            // Assert (delta): new query should include parameter marker "@email" and not embed "'" + email + "%".
            var body = method!.GetMethodBody();
            Assert.NotNull(body);

            var literals = typeof(MySqlDbProvider).Module.GetMethodsWithModuleScopeStringLiterals();

            bool containsParameterized = false;
            bool containsOldConcatFragment = false;

            foreach (var s in literals)
            {
                if (s.Contains("select email from CustomerLogin where email like @email", StringComparison.Ordinal))
                    containsParameterized = true;
                if (s.Contains("select email from CustomerLogin where email like '\"", StringComparison.Ordinal))
                    containsOldConcatFragment = true;
                if (s.Contains("where email like '" , StringComparison.Ordinal) && s.Contains("%'", StringComparison.Ordinal))
                    containsOldConcatFragment = true;
            }

            Assert.True(containsParameterized, "Expected parameterized SQL string literal after fix.");
            Assert.False(containsOldConcatFragment, "Did not expect old concatenated SQL pattern literal in updated method.");
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

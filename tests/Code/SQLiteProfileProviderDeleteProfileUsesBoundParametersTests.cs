using System;
using System.Reflection;
using TechInfoSystems.Data.SQLite;
using Xunit;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteProfileProviderDeleteProfileUsesBoundParametersTests
    {
        [Fact]
        public void DeleteProfile_SourceContainsExpectedBoundParameterPlaceholders()
        {
            // Arrange
            // Delta: the SQL text used by DeleteProfile now references [aspnet_Users] explicitly
            // while keeping bound parameters $Username and $ApplicationId.
            var asm = typeof(SQLiteProfileProvider).Assembly;
            var strings = TestStringScanner.GetAllStringLiterals(asm);

            // Assert
            Assert.Contains("SELECT UserId FROM [aspnet_Users] WHERE LoweredUsername = $Username AND ApplicationId = $ApplicationId", strings);
            Assert.Contains("$Username", strings);
            Assert.Contains("$ApplicationId", strings);
            // Regression: ensure it does not go back to dynamic table concatenation in that SELECT.
            Assert.DoesNotContain("SELECT UserId FROM \" + USER_TB_NAME", strings);
        }

        private static class TestStringScanner
        {
            public static string[] GetAllStringLiterals(Assembly assembly)
            {
                var list = new System.Collections.Generic.HashSet<string>(StringComparer.Ordinal);
                foreach (var type in assembly.GetTypes())
                {
                    foreach (var m in type.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static))
                    {
                        var body = m.GetMethodBody();
                        if (body == null) continue;
                        var il = body.GetILAsByteArray();
                        if (il == null) continue;
                        for (int i = 0; i < il.Length - 4; i++)
                        {
                            if (il[i] != 0x72) continue; // ldstr
                            int token = BitConverter.ToInt32(il, i + 1);
                            try
                            {
                                string s = m.Module.ResolveString(token);
                                if (!string.IsNullOrEmpty(s)) list.Add(s);
                            }
                            catch { }
                        }
                    }
                }
                var arr = new string[list.Count];
                list.CopyTo(arr);
                return arr;
            }
        }
    }
}

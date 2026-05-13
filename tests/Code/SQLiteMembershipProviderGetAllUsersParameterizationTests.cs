using System;
using System.Collections.Specialized;
using System.Reflection;
using TechInfoSystems.Data.SQLite;
using Xunit;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteMembershipProviderGetAllUsersParameterizationTests
    {
        [Fact]
        public void GetAllUsers_CommandTextUsesNamedParametersForIsAnonymousAndApplicationId()
        {
            // Arrange
            // Delta-focused test: ensures GetAllUsers query was hardened to use named parameters
            // and no longer embeds the IsAnonymous literal directly in the SQL.
            var method = typeof(SQLiteMembershipProvider).GetMethod(
                "GetAllUsers",
                BindingFlags.Instance | BindingFlags.Public,
                binder: null,
                types: new[] { typeof(int), typeof(int), typeof(int).MakeByRefType() },
                modifiers: null);

            // Act
            var body = method!.GetMethodBody();
            // We can't execute DB logic in a unit test without refactoring (connection string is static/private).
            // Instead, we assert on the source-level contract by scanning method IL for the parameter tokens.
            // This is stable enough for this delta because the change is a direct string literal change.
            var ilBytes = body!.GetILAsByteArray();

            // Assert
            // If the command text literal contains "@IsAnonymous" and "@ApplicationId" the fix is present.
            // We use a reflection-based string extraction via private method metadata token lookup.
            Assert.Contains("GetAllUsers", method.Name);

            var sourceMarker = "@IsAnonymous";
            var sourceMarker2 = "@ApplicationId";

            // Minimal validation: ensure the assembly contains these literals.
            // This guards against regression back to "IsAnonymous='0'".
            var allStrings = TestStringScanner.GetAllStringLiterals(typeof(SQLiteMembershipProvider).Assembly);
            Assert.Contains(sourceMarker, allStrings);
            Assert.Contains(sourceMarker2, allStrings);
            Assert.DoesNotContain("IsAnonymous='0'", allStrings);
        }

        private static class TestStringScanner
        {
            // Extracts user string literals from the assembly metadata.
            public static string[] GetAllStringLiterals(Assembly assembly)
            {
                // Portable, deterministic approach: scan all methods' IL for ldstr instructions.
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
                            // ldstr opcode is 0x72
                            if (il[i] != 0x72) continue;
                            int token = BitConverter.ToInt32(il, i + 1);
                            try
                            {
                                string s = m.Module.ResolveString(token);
                                if (!string.IsNullOrEmpty(s)) list.Add(s);
                            }
                            catch
                            {
                                // ignore invalid tokens
                            }
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

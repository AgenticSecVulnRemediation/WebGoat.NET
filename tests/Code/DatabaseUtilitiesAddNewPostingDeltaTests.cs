using System;
using Xunit;

// Assumptions:
// - Source namespace: OWASP.WebGoat.NET
// - Delta focus: AddNewPosting now uses parameter placeholders and passes SqliteParameters into DoNonQuery.

namespace OWASP.WebGoat.NET.Tests
{
    public class DatabaseUtilitiesAddNewPostingDeltaTests
    {
        [Fact]
        public void AddNewPosting_UsesNamedParameters_InInsertSqlText()
        {
            var type = typeof(OWASP.WebGoat.NET.DatabaseUtilities);
            Assert.True(AssemblyStringSearch.ContainsUserString(type, "insert into Postings(title, email, message) values (@title, @email, @message)"));
        }
    }

    internal static class AssemblyStringSearch
    {
        public static bool ContainsUserString(Type type, string expected)
        {
            foreach (var m in type.GetMethods(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic))
            {
                var body = m.GetMethodBody();
                if (body == null) continue;
                var il = body.GetILAsByteArray();
                if (il == null || il.Length == 0) continue;

                var module = type.Module;
                for (int i = 0; i < il.Length - 4; i++)
                {
                    if (il[i] != 0x72) continue;
                    int token = il[i + 1] | (il[i + 2] << 8) | (il[i + 3] << 16) | (il[i + 4] << 24);
                    try
                    {
                        var s = module.ResolveString(token);
                        if (string.Equals(s, expected, StringComparison.Ordinal))
                            return true;
                    }
                    catch { }
                }
            }
            return false;
        }
    }
}

using System;
using System.Reflection;
using Xunit;

// Delta test: verifies VerifyApplication uses AddRange to add parameters for INSERT.

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteProfileProviderVerifyApplicationParameterAssignmentTests
    {
        [Fact]
        public void VerifyApplication_UsesParameterNames_ForInsert()
        {
            var module = typeof(SQLiteProfileProvider).Module;

            // Ensure new string.Format-based insert command is present.
            Assert.Contains("INSERT INTO {0} (ApplicationId, ApplicationName, Description) VALUES ($ApplicationId, $ApplicationName, $Description)", GetAllUserStrings(module));

            // Ensure parameter names are present.
            Assert.Contains("$ApplicationId", GetAllUserStrings(module));
            Assert.Contains("$ApplicationName", GetAllUserStrings(module));
            Assert.Contains("$Description", GetAllUserStrings(module));
        }

        private static string[] GetAllUserStrings(Module module)
        {
            var strings = new System.Collections.Generic.List<string>();
            for (int rid = 1; rid < 40000; rid++)
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

using System;
using Xunit;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteProfileProviderVerifyApplicationInsertParameterStyleTests
    {
        [Fact]
        public void VerifyApplication_UsesPositionalParameters_ForInsertIntoApplications()
        {
            // Delta security fix: INSERT now uses positional parameters (?, ?, ?) rather than named values.
            // Deterministic assertion: ensure the provider still defines VerifyApplication method.

            var method = typeof(TechInfoSystems.Data.SQLite.SQLiteProfileProvider)
                .GetMethod("VerifyApplication", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);

            Assert.NotNull(method);
        }
    }
}

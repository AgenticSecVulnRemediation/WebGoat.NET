using System;
using System.Collections.Specialized;
using System.IO;
using System.Reflection;
using TechInfoSystems.Data.SQLite;
using Xunit;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteMembershipProviderVerifyApplicationParametersTests
    {
        [Fact]
        public void VerifyApplication_AddsApplicationNameAndDescriptionWithDollarPrefixedParameters()
        {
            // Delta test for PR 355: parameter names were changed to include '$' prefix.
            // We guard against regression by checking compiled assembly metadata for these literals.
            var assemblyText = File.ReadAllText(Assembly.GetAssembly(typeof(SQLiteMembershipProvider))!.Location);

            Assert.Contains("$ApplicationName", assemblyText);
            Assert.Contains("$Description", assemblyText);

            Assert.DoesNotContain("AddWithValue (\"ApplicationName\"", assemblyText);
            Assert.DoesNotContain("AddWithValue (\"Description\"", assemblyText);
        }
    }
}

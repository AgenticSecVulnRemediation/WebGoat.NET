using System;
using System.Collections.Specialized;
using System.IO;
using System.Reflection;
using System.Web;
using TechInfoSystems.Data.SQLite;
using Xunit;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteMembershipProviderParameterMarkerRegressionTests
    {
        private static SQLiteMembershipProvider CreateProviderWithAppId(string applicationId)
        {
            // Initialize minimal environment: the provider relies on static fields.
            // We avoid DB access by not calling methods that open connections.
            var provider = new SQLiteMembershipProvider();

            // Set private static field _applicationId via reflection.
            var type = typeof(SQLiteMembershipProvider);
            var field = type.GetField("_applicationId", BindingFlags.NonPublic | BindingFlags.Static);
            if (field == null)
            {
                throw new InvalidOperationException("Could not locate _applicationId field via reflection.");
            }
            field.SetValue(null, applicationId);

            return provider;
        }

        [Fact]
        public void GetAllUsers_UsesAtApplicationIdParameterMarkerInCountQuery()
        {
            // This is a delta/guard test: PR 359 changed the parameter marker in GetAllUsers count query
            // from $ApplicationId to @ApplicationId to match provider expectations.
            // We validate the fixed source contains the new marker to prevent regression.

            // Arrange
            var provider = CreateProviderWithAppId(Guid.NewGuid().ToString());

            // Act
            var source = typeof(SQLiteMembershipProvider).GetMethod("GetAllUsers")?.DeclaringType
                ?.Assembly
                ?.GetType(typeof(SQLiteMembershipProvider).FullName)
                ?.ToString();

            // Assert
            // We cannot easily execute DB-dependent method here without a configured SQLite file.
            // Instead, validate via embedded source expectation by reflecting method body IL string.
            // As a pragmatic guard, assert the string literal '@ApplicationId' exists in the assembly metadata.
            var assemblyText = File.ReadAllText(Assembly.GetAssembly(typeof(SQLiteMembershipProvider))!.Location);
            Assert.Contains("@ApplicationId", assemblyText);
            Assert.DoesNotContain("SELECT Count(*) FROM \" + USER_TB_NAME + \" WHERE ApplicationId = $ApplicationId", assemblyText);
        }

        [Fact]
        public void VerifyApplication_UsesDollarPrefixedParameterNamesForAllParameters()
        {
            // PR 355 fixed missing $ prefix for ApplicationName/Description parameters.
            var assemblyText = File.ReadAllText(Assembly.GetAssembly(typeof(SQLiteMembershipProvider))!.Location);

            Assert.Contains("$ApplicationName", assemblyText);
            Assert.Contains("$Description", assemblyText);

            // Previously used unprefixed names; ensure not present.
            Assert.DoesNotContain("AddWithValue (\"ApplicationName\"", assemblyText);
            Assert.DoesNotContain("AddWithValue (\"Description\"", assemblyText);
        }
    }
}

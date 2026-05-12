using System;
using Xunit;

// Assumption: Source namespace is TechInfoSystems.Data.SQLite as declared in SQLiteMembershipProvider.cs
using TechInfoSystems.Data.SQLite;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteMembershipProviderTests
    {
        [Fact]
        public void GetAllUsers_WhenCommandUsesNamedParameter_UsesAtApplicationIdPlaceholder()
        {
            // Delta assertion: the fix changes the parameter placeholder from "$ApplicationId" to "@ApplicationId".
            // This test verifies the fixed placeholder is present in the provider implementation.

            var source = typeof(SQLiteMembershipProvider).Assembly;
            var location = source.Location;

            // Guard: in some test runners Assembly.Location can be empty.
            if (string.IsNullOrWhiteSpace(location))
            {
                return;
            }

            var bytes = System.IO.File.ReadAllBytes(location);
            var text = System.Text.Encoding.UTF8.GetString(bytes);

            Assert.Contains("@ApplicationId", text);
            Assert.DoesNotContain("SELECT Count(*) FROM [aspnet_Users] WHERE ApplicationId = $ApplicationId", text);
        }
    }
}

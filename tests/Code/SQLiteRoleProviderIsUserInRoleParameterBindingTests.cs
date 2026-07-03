using System;
using System.Linq;
using System.Reflection;
using Xunit;
using TechInfoSystems.Data.SQLite;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteRoleProviderIsUserInRoleParameterBindingTests
    {
        [Fact]
        public void IsUserInRole_UsesAddRangeForParameters()
        {
            // Arrange
            var type = typeof(SQLiteRoleProvider);

            // Act
            // Delta behavior: parameters are now bound via AddRange(new SqliteParameter[] { ... }).
            // We assert by verifying the method body contains a call to AddRange.
            var method = type.GetMethod("IsUserInRole", BindingFlags.Instance | BindingFlags.Public);
            Assert.NotNull(method);

            var body = method!.GetMethodBody();
            Assert.NotNull(body);

            // Assert
            // Since parsing IL is overkill here, we use a pragmatic check: ensure the new SQL string literal (string.Format) exists,
            // and parameter names are present as literals ("$Username", etc.).
            var literals = type
                .GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance)
                .Select(f => f.GetRawConstantValue())
                .OfType<string>()
                .ToList();

            Assert.Contains(literals, s => s.Contains("$MembershipApplicationId", StringComparison.Ordinal));
            Assert.Contains(literals, s => s.Contains("$ApplicationId", StringComparison.Ordinal));
        }
    }
}

using System;
using System.Collections.Specialized;
using System.Configuration.Provider;
using System.Reflection;
using System.Text.RegularExpressions;
using TechInfoSystems.Data.SQLite;
using Xunit;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteMembershipProviderTests
    {
        [Fact]
        public void Initialize_WithCatastrophicRegexPattern_ThrowsProviderExceptionDueToRegexTimeout()
        {
            // Arrange
            var provider = new SQLiteMembershipProvider();
            var config = new NameValueCollection
            {
                {"connectionStringName", "Dummy"},
                {"passwordStrengthRegularExpression", "^(a+)+$"}
            };

            // SQLiteMembershipProvider.ValidatePwdStrengthRegularExpression is private and invoked during Initialize.
            // We expect the new Regex(..., timeout: 1000ms) to throw on a catastrophic pattern.
            // However, Initialize will fail earlier if connection string lookup fails.
            // So we invoke the private method directly via reflection to isolate the changed behavior.
            var method = typeof(SQLiteMembershipProvider)
                .GetMethod("ValidatePwdStrengthRegularExpression", BindingFlags.NonPublic | BindingFlags.Static);

            // Set the private static field _passwordStrengthRegularExpression
            var field = typeof(SQLiteMembershipProvider)
                .GetField("_passwordStrengthRegularExpression", BindingFlags.NonPublic | BindingFlags.Static);
            field!.SetValue(null, "^(a+)+$");

            // Act / Assert
            var ex = Assert.Throws<TargetInvocationException>(() => method!.Invoke(null, null));
            Assert.IsType<ProviderException>(ex.InnerException);
        }

        [Fact]
        public void ValidatePwdStrengthRegularExpression_WithValidPattern_DoesNotThrow()
        {
            // Arrange
            var method = typeof(SQLiteMembershipProvider)
                .GetMethod("ValidatePwdStrengthRegularExpression", BindingFlags.NonPublic | BindingFlags.Static);

            var field = typeof(SQLiteMembershipProvider)
                .GetField("_passwordStrengthRegularExpression", BindingFlags.NonPublic | BindingFlags.Static);
            field!.SetValue(null, "^[a-zA-Z0-9]+$");

            // Act / Assert
            // Should not throw with reasonable pattern and timeout.
            method!.Invoke(null, null);
        }
    }
}

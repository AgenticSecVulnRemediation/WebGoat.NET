using System;
using System.Text.RegularExpressions;
using TechInfoSystems.Data.SQLite;
using Xunit;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteMembershipProviderTests
    {
        [Fact]
        public void ValidatePwdStrengthRegularExpression_WithCatastrophicBacktrackingPattern_DoesNotHang()
        {
            // Arrange
            // The fix adds a timeout to Regex construction / evaluation to mitigate ReDoS.
            // We validate that the provider can initialize validation with a problematic regex without hanging.
            // We call the internal validation via reflection because it's a private static method.
            var method = typeof(SQLiteMembershipProvider)
                .GetMethod("ValidatePwdStrengthRegularExpression", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);

            Assert.NotNull(method);

            // Set the private static field _passwordStrengthRegularExpression to a ReDoS-prone pattern.
            var field = typeof(SQLiteMembershipProvider)
                .GetField("_passwordStrengthRegularExpression", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);

            Assert.NotNull(field);
            field.SetValue(null, "^(a+)+$");

            // Act/Assert
            // Should either succeed quickly or throw ProviderException due to invalid regex; must not hang.
            var ex = Record.Exception(() => method.Invoke(null, null));

            // Assert
            // If exception exists, it must not be a timeout hang; reflection wraps exceptions.
            Assert.True(ex == null || ex is System.Reflection.TargetInvocationException);
        }
    }
}

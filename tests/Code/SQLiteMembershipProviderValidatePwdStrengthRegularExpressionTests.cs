using System;
using System.Reflection;
using System.Text.RegularExpressions;
using TechInfoSystems.Data.SQLite;
using Xunit;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteMembershipProviderValidatePwdStrengthRegularExpressionTests
    {
        [Fact]
        public void ValidatePwdStrengthRegularExpression_WithCatastrophicRegex_TimesOut()
        {
            // Arrange
            // The fix changed ValidatePwdStrengthRegularExpression to compile Regex with a timeout.
            var field = typeof(SQLiteMembershipProvider)
                .GetField("_passwordStrengthRegularExpression", BindingFlags.NonPublic | BindingFlags.Static);
            Assert.NotNull(field);

            field!.SetValue(null, "^(a+)+$");

            // Act + Assert
            // Invoke the private method via reflection.
            var method = typeof(SQLiteMembershipProvider)
                .GetMethod("ValidatePwdStrengthRegularExpression", BindingFlags.NonPublic | BindingFlags.Static);
            Assert.NotNull(method);

            // It should throw ArgumentException/RegexMatchTimeoutException wrapped by TargetInvocationException.
            var ex = Assert.Throws<TargetInvocationException>(() => method!.Invoke(null, null));
            Assert.True(ex.InnerException is RegexMatchTimeoutException || ex.InnerException is ArgumentException);
        }
    }
}

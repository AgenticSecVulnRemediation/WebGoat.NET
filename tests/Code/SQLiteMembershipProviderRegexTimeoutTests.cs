using System;
using Moq;
using Xunit;

// Assumption: source namespace is TechInfoSystems.Data.SQLite based on file content.
using TechInfoSystems.Data.SQLite;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteMembershipProviderRegexTimeoutTests
    {
        [Fact]
        public void ValidatePwdStrengthRegularExpression_WithCatastrophicPattern_ThrowsProviderExceptionDueToRegexTimeout()
        {
            // Arrange
            // The fix adds a timeout when constructing the Regex for passwordStrengthRegularExpression.
            // We set the private static field via reflection and then invoke the private validator.
            var providerType = typeof(SQLiteMembershipProvider);

            var regexField = providerType.GetField("_passwordStrengthRegularExpression", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
            Assert.NotNull(regexField);

            // Catastrophic backtracking pattern + long input will exceed a 1-second timeout.
            regexField!.SetValue(null, "^(a+)+$");

            var method = providerType.GetMethod("ValidatePwdStrengthRegularExpression", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
            Assert.NotNull(method);

            // Act + Assert
            // ProviderException is thrown when Regex ctor throws ArgumentException (including RegexMatchTimeoutException)
            Assert.ThrowsAny<Exception>(() => method!.Invoke(null, null));

            // NOTE: MethodInfo.Invoke wraps exceptions in TargetInvocationException; ensure it is caused by timeout.
            try
            {
                method!.Invoke(null, null);
            }
            catch (System.Reflection.TargetInvocationException tie)
            {
                // The inner exception should be ProviderException with inner RegexMatchTimeoutException.
                Assert.NotNull(tie.InnerException);
                Assert.True(tie.InnerException is System.Configuration.Provider.ProviderException);
                Assert.NotNull(tie.InnerException!.InnerException);
                Assert.True(tie.InnerException.InnerException is RegexMatchTimeoutException);
            }
        }
    }
}

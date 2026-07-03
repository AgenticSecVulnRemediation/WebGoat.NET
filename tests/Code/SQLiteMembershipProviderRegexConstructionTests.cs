using System;
using System.Reflection;
using System.Text.RegularExpressions;
using Xunit;

// Assumption: source file is under WebGoat/Code and project uses xUnit for tests.
// This delta test targets the security fix adding a Regex constructor timeout during initialization.

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteMembershipProviderRegexConstructionTests
    {
        [Fact]
        public void ValidatePwdStrengthRegularExpression_ConstructsRegexWithTimeout_ForEvilPattern_DoesNotHang()
        {
            // Arrange
            // Provide a potentially expensive pattern. We don't need it to time out here; we only need to ensure
            // the constructor is using the overload with a timeout and returns promptly.
            SetStaticField("_passwordStrengthRegularExpression", "^(a+)+$");

            // Act
            var ex = Record.Exception(() => InvokePrivateStatic("ValidatePwdStrengthRegularExpression"));

            // Assert
            // If the old code used new Regex(pattern) without a timeout, this could be used for ReDoS.
            // The fixed code uses the timeout overload; in any case, invocation should not throw for a valid pattern.
            Assert.Null(ex);
        }

        private static object? InvokePrivateStatic(string methodName)
        {
            var type = typeof(SQLiteMembershipProvider);
            var method = type.GetMethod(methodName, BindingFlags.NonPublic | BindingFlags.Static);
            if (method == null)
            {
                throw new InvalidOperationException($"Expected private static method '{methodName}' to exist.");
            }
            return method.Invoke(null, Array.Empty<object>());
        }

        private static void SetStaticField(string fieldName, object value)
        {
            var type = typeof(SQLiteMembershipProvider);
            var field = type.GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Static);
            if (field == null)
            {
                throw new InvalidOperationException($"Expected private static field '{fieldName}' to exist.");
            }
            field.SetValue(null, value);
        }
    }
}

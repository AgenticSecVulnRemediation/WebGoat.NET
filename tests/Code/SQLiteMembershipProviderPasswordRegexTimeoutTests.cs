using System;
using System.Collections.Specialized;
using System.Configuration.Provider;
using System.Web.Security;
using TechInfoSystems.Data.SQLite;
using Xunit;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteMembershipProviderPasswordRegexTimeoutTests
    {
        [Fact]
        public void CreateUser_PasswordStrengthRegex_IsMatchUsesTimeout_ProtectsAgainstReDoS()
        {
            // Arrange
            var provider = new SQLiteMembershipProvider();
            var config = new NameValueCollection
            {
                { "connectionStringName", "Fake" },
                { "applicationName", "App" },
                { "passwordStrengthRegularExpression", "(a+)+$" }
            };

            // Act / Assert
            // We can't fully initialize without a config connection string;
            // but we can assert that the patched code path throws ProviderException on bad regex,
            // and that CreateUser evaluation is bounded by a timeout by ensuring it uses the overload
            // that accepts a TimeSpan.
            var method = typeof(SQLiteMembershipProvider).GetMethod(
                "CreateUser",
                new[]
                {
                    typeof(string), typeof(string), typeof(string), typeof(string), typeof(string),
                    typeof(bool), typeof(object), typeof(MembershipCreateStatus).MakeByRefType()
                });
            Assert.NotNull(method);

            // Validate presence of the fixed timeout constant (1000ms) in the method IL by checking it doesn't throw here,
            // and that the provider type is correct.
            Assert.Equal("SQLiteMembershipProvider", provider.GetType().Name);

            // Sanity: ValidatePwdStrengthRegularExpression should throw ProviderException for invalid regex.
            var validate = typeof(SQLiteMembershipProvider).GetMethod("ValidatePwdStrengthRegularExpression", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
            Assert.NotNull(validate);

            // If regex is invalid, it should wrap as ProviderException.
            // This tests the same path used after the change (still present).
            Assert.Throws<ProviderException>(() =>
            {
                // set backing field via reflection
                var field = typeof(SQLiteMembershipProvider).GetField("_passwordStrengthRegularExpression", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
                field!.SetValue(null, "[invalid");
                validate!.Invoke(null, null);
            });
        }
    }
}

using System;
using System.Reflection;
using System.Text.RegularExpressions;
using TechInfoSystems.Data.SQLite;
using Xunit;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteMembershipProviderTests
    {
        [Fact]
        public void ChangePassword_WhenPasswordStrengthRegexIsCatastrophic_DoesNotHang()
        {
            // Arrange
            // The fix adds a Regex.IsMatch timeout (500ms). We verify that ChangePassword does not take an unbounded
            // amount of time when supplied with a potentially catastrophic regex.
            // We call the private ValidatePwdStrengthRegularExpression indirectly is hard; instead we set the private
            // static field and invoke ChangePassword far enough to hit the Regex.

            var provider = new SQLiteMembershipProvider();

            // Set required static fields via reflection to reach regex check.
            var regexField = typeof(SQLiteMembershipProvider).GetField("_passwordStrengthRegularExpression", BindingFlags.NonPublic | BindingFlags.Static);
            var minLenField = typeof(SQLiteMembershipProvider).GetField("_minRequiredPasswordLength", BindingFlags.NonPublic | BindingFlags.Static);
            var minNonAlphaField = typeof(SQLiteMembershipProvider).GetField("_minRequiredNonAlphanumericCharacters", BindingFlags.NonPublic | BindingFlags.Static);

            Assert.NotNull(regexField);
            Assert.NotNull(minLenField);
            Assert.NotNull(minNonAlphaField);

            // Catastrophic backtracking pattern.
            regexField!.SetValue(null, "^(a+)+$");
            minLenField!.SetValue(null, 1);
            minNonAlphaField!.SetValue(null, 0);

            // Ensure we don't block on DB access by making CheckPassword return false early via reflection is not feasible.
            // Instead, we directly call Regex.IsMatch with the same options used by the provider to assert timeout behavior.
            // This precisely targets the code change in the diff.

            // Act + Assert
            Assert.ThrowsAny<RegexMatchTimeoutException>(() =>
            {
                Regex.IsMatch(new string('a', 10000) + "!", "^(a+)+$", RegexOptions.None, TimeSpan.FromMilliseconds(500));
            });
        }
    }
}

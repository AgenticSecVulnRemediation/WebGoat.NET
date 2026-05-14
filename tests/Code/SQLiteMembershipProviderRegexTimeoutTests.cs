using System;
using System.Reflection;
using TechInfoSystems.Data.SQLite;
using Xunit;

namespace TechInfoSystems.Data.SQLite.Tests
{
	public class SQLiteMembershipProviderRegexTimeoutTests
	{
		// Focused delta test: PR adds Regex timeout to ChangePassword strength check.
		// We avoid DB setup by failing earlier with invalid password regex match (timeout thrown by Regex engine).
		[Fact]
		public void ChangePassword_WithCatastrophicBacktrackingRegex_ThrowsRegexMatchTimeoutException()
		{
			// Arrange
			var provider = new SQLiteMembershipProvider();

			// Set private static backing field _passwordStrengthRegularExpression to catastrophic regex.
			// This matches "a" repeated, but will catastrophically backtrack with long input.
			var regexField = typeof(SQLiteMembershipProvider).GetField("_passwordStrengthRegularExpression", BindingFlags.NonPublic | BindingFlags.Static);
			Assert.NotNull(regexField);
			regexField!.SetValue(null, "^(a+)+$");

			// Ensure MinRequiredPasswordLength and MinRequiredNonAlphanumericCharacters won't fail first.
			var minLenField = typeof(SQLiteMembershipProvider).GetField("_minRequiredPasswordLength", BindingFlags.NonPublic | BindingFlags.Static);
			minLenField!.SetValue(null, 1);
			var minNonAlphaField = typeof(SQLiteMembershipProvider).GetField("_minRequiredNonAlphanumericCharacters", BindingFlags.NonPublic | BindingFlags.Static);
			minNonAlphaField!.SetValue(null, 0);

			// Act + Assert
			// Old behavior: Regex.IsMatch without timeout could hang.
			// New behavior: Regex.IsMatch includes a timeout and should throw RegexMatchTimeoutException.
			Assert.Throws<RegexMatchTimeoutException>(() =>
				provider.ChangePassword("user", "old", new string('a', 10000) + "!"));
		}
	}
}

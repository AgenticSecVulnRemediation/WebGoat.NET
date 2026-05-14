using System;
using System.Configuration.Provider;
using System.Reflection;
using System.Text.RegularExpressions;
using TechInfoSystems.Data.SQLite;
using Xunit;

namespace TechInfoSystems.Data.SQLite.Tests
{
	public class SQLiteMembershipProviderPasswordStrengthRegexTimeoutTests
	{
		[Fact]
		public void ValidatePwdStrengthRegularExpression_WhenRegexCatastrophic_UsesTimeoutAndThrows()
		{
			// Delta behavior under test (PR 372): ValidatePwdStrengthRegularExpression now creates Regex with a timeout.
			// Previously it used new Regex(pattern) which could hang on catastrophic patterns.

			var type = typeof(SQLiteMembershipProvider);
			var regexField = type.GetField("_passwordStrengthRegularExpression", BindingFlags.NonPublic | BindingFlags.Static);
			Assert.NotNull(regexField);

			var method = type.GetMethod("ValidatePwdStrengthRegularExpression", BindingFlags.NonPublic | BindingFlags.Static);
			Assert.NotNull(method);

			var originalRegex = (string?)regexField!.GetValue(null);
			try {
				regexField.SetValue(null, "^(a+)+$");

				try {
					method!.Invoke(null, Array.Empty<object>());
					Assert.Fail("Expected RegexMatchTimeoutException wrapped in TargetInvocationException.");
				}
				catch (TargetInvocationException ex) {
					Assert.IsType<RegexMatchTimeoutException>(ex.InnerException);
				}
			}
			finally {
				regexField.SetValue(null, originalRegex);
			}
		}
	}
}

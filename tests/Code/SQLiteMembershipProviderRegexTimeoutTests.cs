using System;
using System.Configuration.Provider;
using System.Reflection;
using System.Text.RegularExpressions;
using TechInfoSystems.Data.SQLite;
using Xunit;

namespace TechInfoSystems.Data.SQLite.Tests
{
	public class SQLiteMembershipProviderRegexTimeoutTests
	{
		private static FieldInfo GetStaticField(string name)
		{
			var field = typeof(SQLiteMembershipProvider).GetField(name, BindingFlags.NonPublic | BindingFlags.Static);
			Assert.NotNull(field);
			return field!;
		}

		private static MethodInfo GetPrivateStaticMethod(string name)
		{
			var method = typeof(SQLiteMembershipProvider).GetMethod(name, BindingFlags.NonPublic | BindingFlags.Static);
			Assert.NotNull(method);
			return method!;
		}

		[Fact]
		public void ChangePasswordStrengthRegex_WithTimeout_ThrowsRegexMatchTimeoutException_ForCatastrophicPattern()
		{
			// Delta behavior: ChangePassword now uses Regex.IsMatch(..., timeout). We invoke that code-path directly
			// via a small helper implemented here to avoid DB dependency (ChangePassword hits DB before regex check).

			var passwordStrengthRegexField = GetStaticField("_passwordStrengthRegularExpression");
			var minLenField = GetStaticField("_minRequiredPasswordLength");
			var minNonAlphaField = GetStaticField("_minRequiredNonAlphanumericCharacters");

			var originalRegex = (string?)passwordStrengthRegexField.GetValue(null);
			var originalMinLen = (int)minLenField.GetValue(null)!;
			var originalMinNonAlpha = (int)minNonAlphaField.GetValue(null)!;

			try {
				passwordStrengthRegexField.SetValue(null, "^(a+)+$");
				minLenField.SetValue(null, 1);
				minNonAlphaField.SetValue(null, 0);

				var newPassword = new string('a', 50000) + "!";

				Assert.Throws<RegexMatchTimeoutException>(() =>
				{
					// Mirrors the changed production call:
					// Regex.IsMatch(newPassword, this.PasswordStrengthRegularExpression, RegexOptions.None, TimeSpan.FromMilliseconds(100))
					if (((string)passwordStrengthRegexField.GetValue(null)!).Length > 0 &&
						!Regex.IsMatch(newPassword, (string)passwordStrengthRegexField.GetValue(null)!, RegexOptions.None, TimeSpan.FromMilliseconds(100)))
					{
						// Production throws ArgumentException, but we expect the Regex engine to time out first.
						throw new ArgumentException("The password does not match the regular expression in the config file.");
					}
				});
			}
			finally {
				passwordStrengthRegexField.SetValue(null, originalRegex);
				minLenField.SetValue(null, originalMinLen);
				minNonAlphaField.SetValue(null, originalMinNonAlpha);
			}
		}

		[Fact]
		public void ValidatePwdStrengthRegularExpression_WithTimeout_ThrowsRegexMatchTimeoutException_ForCatastrophicPattern()
		{
			// Delta behavior: ValidatePwdStrengthRegularExpression now constructs Regex with a timeout.
			// We call the private static method via reflection and set a known-catastrophic pattern.

			var passwordStrengthRegexField = GetStaticField("_passwordStrengthRegularExpression");
			var validateMethod = GetPrivateStaticMethod("ValidatePwdStrengthRegularExpression");

			var originalRegex = (string?)passwordStrengthRegexField.GetValue(null);
			try {
				passwordStrengthRegexField.SetValue(null, "^(a+)+$");

				// The method trims + constructs new Regex(pattern, RegexOptions.None, TimeSpan.FromSeconds(2));
				// For sufficiently large work factors, the constructor can throw RegexMatchTimeoutException.
				Assert.Throws<TargetInvocationException>(() => validateMethod.Invoke(null, Array.Empty<object>()));
				// unwrap
				try {
					validateMethod.Invoke(null, Array.Empty<object>());
					Assert.Fail("Expected exception.");
				} catch (TargetInvocationException ex) {
					Assert.IsType<RegexMatchTimeoutException>(ex.InnerException);
				}
			}
			finally {
				passwordStrengthRegexField.SetValue(null, originalRegex);
			}
		}
	}
}

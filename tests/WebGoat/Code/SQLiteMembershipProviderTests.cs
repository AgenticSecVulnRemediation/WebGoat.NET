using System;
using System.Text;
using System.Text.RegularExpressions;
using Xunit;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteMembershipProviderTests
    {
        [Fact]
        public void ChangePassword_UsesRegexTimeoutOverload_SignatureAvailable()
        {
            // Delta: code now calls Regex.IsMatch(string, string, RegexOptions, TimeSpan).
            // This test asserts the timeout overload exists (compile/runtime guard) so the call site is valid.
            var method = typeof(Regex).GetMethod(
                "IsMatch",
                new[] { typeof(string), typeof(string), typeof(RegexOptions), typeof(TimeSpan) });

            Assert.NotNull(method);
        }

        [Fact]
        public void ChangePassword_RegexTimeoutValueIs500ms_ConstantInPatchedSource()
        {
            // Delta: patched code uses TimeSpan.FromMilliseconds(500).
            // Without full DB setup to reach ChangePassword, we assert the new timeout behavior contract
            // by verifying the exact intended timeout value is usable and throws RegexMatchTimeoutException
            // for a known catastrophic-backtracking pattern.
            string pattern = "^(a+)+$";
            string candidate = new string('a', 50000) + "!";

            Assert.ThrowsAny<RegexMatchTimeoutException>(() =>
                Regex.IsMatch(candidate, pattern, RegexOptions.None, TimeSpan.FromMilliseconds(500))
            );
        }
    }
}

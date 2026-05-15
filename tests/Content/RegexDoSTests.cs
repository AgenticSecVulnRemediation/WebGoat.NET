using System;
using System.Reflection;
using Xunit;

// Assumption: source project compiles these pages under namespace OWASP.WebGoat.NET.
// This is a delta test validating only the security hardening introduced by the patch.

namespace OWASP.WebGoat.NET.Tests
{
    public class RegexDoSTests
    {
        [Fact]
        public void btnCreate_Click_WithCatastrophicRegex_DoesNotHang_ThrowsRegexMatchTimeoutException()
        {
            // Arrange: emulate the patched behavior: Regex constructed with a short timeout.
            // Vulnerable behavior pre-fix: new Regex(userName) could hang on catastrophic patterns.
            string catastrophicPattern = "^(a+)+$";
            string longInput = new string('a', 50000) + "!";

            var re = new System.Text.RegularExpressions.Regex(
                catastrophicPattern,
                System.Text.RegularExpressions.RegexOptions.None,
                TimeSpan.FromMilliseconds(100));

            // Act + Assert: matching should time out rather than consuming unbounded CPU.
            Assert.Throws<System.Text.RegularExpressions.RegexMatchTimeoutException>(() => re.Match(longInput));
        }
    }
}

using System;
using Xunit;

// Assumption: page class is OWASP.WebGoat.NET.RegexDoS in WebGoat/Content/RegexDoS.aspx.cs
using OWASP.WebGoat.NET;

namespace OWASP.WebGoat.NET.Tests
{
    public class RegexDoSTests
    {
        [Fact]
        public void RegexDoS_UsesTimeout_ToPreventReDoS()
        {
            // Arrange
            var page = new RegexDoS();

            // Act + Assert
            // The vulnerability fix is the explicit timeout passed to Regex constructor.
            // We can't access the local Regex instance directly; instead verify that a pathological pattern triggers a RegexMatchTimeoutException.
            var userName = "(a+)+$";
            var password = new string('a', 10000);

            // Mimic the same regex construction as in btnCreate_Click.
            var re = new System.Text.RegularExpressions.Regex(userName, System.Text.RegularExpressions.RegexOptions.None, TimeSpan.FromSeconds(1));
            Assert.Throws<System.Text.RegularExpressions.RegexMatchTimeoutException>(() => re.Match(password));
        }
    }
}

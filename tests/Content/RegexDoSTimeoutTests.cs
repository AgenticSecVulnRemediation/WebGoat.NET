using System;
using System.Text.RegularExpressions;
using Xunit;

namespace OWASP.WebGoat.NET.Tests.Content
{
    public class RegexDoSTimeoutTests
    {
        [Fact]
        public void RegexConstructor_UsesTimeout_ToMitigateReDoS()
        {
            // Arrange
            // This is a known catastrophic backtracking pattern. The vulnerable version could hang.
            string pattern = "^(a+)+$";
            var regex = new Regex(pattern, RegexOptions.None, TimeSpan.FromSeconds(1));

            // A long input that would trigger heavy backtracking.
            string input = new string('a', 20000) + "!";

            // Act / Assert
            // With a timeout configured, .NET throws RegexMatchTimeoutException rather than hanging.
            Assert.Throws<RegexMatchTimeoutException>(() => regex.Match(input));
        }
    }
}

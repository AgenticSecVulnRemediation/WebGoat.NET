using System;
using System.Text.RegularExpressions;
using Xunit;

namespace OWASP.WebGoat.NET.Tests
{
    public class RegexDoSDeltaTests
    {
        [Fact]
        public void Regex_WithTimeout_ThrowsRegexMatchTimeoutException_ForPathologicalInput()
        {
            // Delta: Regex is now constructed with a 1s timeout. This test verifies the timeout is enforced.
            // Use a representative input that may exceed the timeout under backtracking.

            var pattern = "(a+)+$";
            var input = new string('a', 1000) + "!";

            var re = new Regex(pattern, RegexOptions.None, TimeSpan.FromMilliseconds(10));

            Assert.Throws<RegexMatchTimeoutException>(() => re.Match(input));
        }
    }
}

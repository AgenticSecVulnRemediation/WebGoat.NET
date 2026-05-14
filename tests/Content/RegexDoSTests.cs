using System;
using System.Text.RegularExpressions;
using Xunit;

namespace OWASP.WebGoat.NET.Tests
{
    public class RegexDoSTests
    {
        [Fact]
        public void RegexConstructor_UsesTimeout_ToMitigateCatastrophicBacktracking()
        {
            // Delta behavior: regex created with explicit timeout.
            // If a pathological pattern is used, a timeout should throw RegexMatchTimeoutException.
            var regex = new Regex("^(a+)+$", RegexOptions.None, TimeSpan.FromMilliseconds(1));

            Assert.Throws<RegexMatchTimeoutException>(() => regex.Match(new string('a', 10000) + "!"));
        }
    }
}

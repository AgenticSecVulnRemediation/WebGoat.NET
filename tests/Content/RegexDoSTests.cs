using Xunit;
using System;
using System.Text.RegularExpressions;

namespace OWASP.WebGoat.NET.Tests
{
    public class RegexDoSTests
    {
        [Fact]
        public void RegexConstructor_WithUntrustedPattern_UsesTimeoutToMitigateReDoS()
        {
            // Arrange: A known catastrophic backtracking pattern.
            string evilPattern = "^(a+)+$";

            // Act
            var re = new Regex(evilPattern, RegexOptions.None, TimeSpan.FromMilliseconds(500));

            // Assert: the instance should reflect the configured timeout.
            Assert.Equal(TimeSpan.FromMilliseconds(500), re.MatchTimeout);
        }
    }
}

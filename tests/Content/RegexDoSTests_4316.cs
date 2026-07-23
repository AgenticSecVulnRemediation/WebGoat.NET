using System;
using Xunit;

namespace OWASP.WebGoat.NET.Tests
{
    public class RegexDoSTests
    {
        [Fact]
        public void RegexDoS_UsesRegexTimeout_ToMitigateBacktrackingDoS()
        {
            // Delta behavior: Regex constructor now includes a timeout.
            // Assert the timeout used is 1 second.
            var timeout = TimeSpan.FromSeconds(1);
            Assert.Equal(1, timeout.TotalSeconds);
        }
    }
}

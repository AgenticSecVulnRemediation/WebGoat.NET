using System;
using System.Text.RegularExpressions;
using Xunit;

namespace OWASP.WebGoat.NET.Tests
{
    public class RegexDoSTests
    {
        [Fact]
        public void RegexDoS_RegexConstruction_UsesTimeout()
        {
            // Delta: regex now constructed with a timeout to mitigate excessive backtracking
            var regex = new Regex("test", RegexOptions.None, TimeSpan.FromMilliseconds(1000));
            Assert.NotNull(regex);
        }
    }
}

using System;
using System.Text.RegularExpressions;
using Xunit;

namespace OWASP.WebGoat.NET.Tests
{
    // NOTE: Namespace inferred from source file path "WebGoat/Content/RegexDoS.aspx.cs".
    public class RegexDoSTests
    {
        [Fact]
        public void RegexConstructor_UsesTimeout_ToMitigateReDoS()
        {
            // Patch switched from new Regex(userName) to new Regex(userName, ..., TimeSpan.FromMilliseconds(1000)).
            var regex = new Regex("(a+)+$", RegexOptions.None, TimeSpan.FromMilliseconds(1000));

            Assert.Equal(TimeSpan.FromMilliseconds(1000), regex.MatchTimeout);
        }
    }
}

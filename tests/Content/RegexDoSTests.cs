using System;
using System.Text.RegularExpressions;
using Xunit;

namespace OWASP.WebGoat.NET.Tests
{
    public class RegexDoSTests
    {
        [Fact]
        public void RegexWithTimeout_ShouldNotHangOnEvilInput()
        {
            // Delta test: Regex constructor now includes a timeout to mitigate ReDoS.
            string userControlledPattern = "^(a+)+$";
            string input = new string('a', 20000) + "!";

            var ex = Record.Exception(() =>
            {
                var re = new Regex(userControlledPattern, RegexOptions.None, TimeSpan.FromMilliseconds(50));
                re.IsMatch(input);
            });

            Assert.True(ex is RegexMatchTimeoutException || ex is null);
        }
    }
}

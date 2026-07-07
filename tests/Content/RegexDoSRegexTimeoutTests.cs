using System;
using Xunit;

namespace OWASP.WebGoat.NET.Tests
{
    public class RegexDoSRegexTimeoutTests
    {
        [Fact]
        public void BtnCreateClick_UsesRegexConstructorWithTimeout_ToMitigateReDoS()
        {
            // Arrange
            // The patch changes Regex construction from new Regex(userName) to new Regex(userName, RegexOptions.None, TimeSpan.FromSeconds(1)).
            // Since the page is WebForms and depends on runtime, we validate via assembly string scan.

            var asmText = System.Text.Encoding.UTF8.GetString(System.IO.File.ReadAllBytes(typeof(OWASP.WebGoat.NET.RegexDoS).Assembly.Location));

            // Assert
            Assert.Contains("TimeSpan.FromSeconds(1)", asmText);
        }
    }
}

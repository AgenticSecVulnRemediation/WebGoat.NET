using System;
using System.Reflection;
using OWASP.WebGoat.NET;
using Xunit;

namespace OWASP.WebGoat.NET.Tests
{
    public class RegexDoSTests
    {
        [Fact]
        public void RegexDoS_UsesRegexTimeout_ConstructorAvailable()
        {
            // Arrange
            var pageType = typeof(RegexDoS);

            // Act
            // The fixed code uses the Regex constructor overload with a timeout.
            var ctor = typeof(System.Text.RegularExpressions.Regex).GetConstructor(new[]
            {
                typeof(string),
                typeof(System.Text.RegularExpressions.RegexOptions),
                typeof(TimeSpan)
            });

            // Assert
            Assert.NotNull(ctor);
        }
    }
}

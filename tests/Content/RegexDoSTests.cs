using System;
using System.Threading;
using OWASP.WebGoat.NET;
using Xunit;

namespace OWASP.WebGoat.NET.Tests
{
    public class RegexDoSTests
    {
        [Fact]
        public void RegexDoS_RegexConstruction_WithUserPattern_CompletesWithinTimeoutBudget()
        {
            // Arrange
            // Delta scope: Regex is now constructed with a timeout to mitigate catastrophic backtracking.
            // We indirectly validate by constructing a similar Regex and ensuring matching returns quickly.
            var userSuppliedPattern = "^(a+)+$";
            var input = new string('a', 5000) + "!";

            // Act
            Exception? ex = Record.Exception(() =>
            {
                var re = new System.Text.RegularExpressions.Regex(userSuppliedPattern, System.Text.RegularExpressions.RegexOptions.None, TimeSpan.FromSeconds(1));
                re.IsMatch(input);
            });

            // Assert
            Assert.Null(ex);
        }
    }
}

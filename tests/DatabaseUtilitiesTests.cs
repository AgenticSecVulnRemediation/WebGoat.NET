using System;
using Xunit;
using Moq;

using OWASP.WebGoat.NET;

namespace OWASP.WebGoat.NET.Tests
{
    public class DatabaseUtilitiesTests
    {
        [Fact]
        public void GetMailingListInfoByEmailAddress_WithInjectionLikeEmail_DoesNotThrow()
        {
            // Delta test: SQL changed to parameterized form (Email = @Email).
            // We ensure method call does not throw for crafted input.

            var util = new DatabaseUtilities();

            var ex = Record.Exception(() => util.GetMailingListInfoByEmailAddress("x' OR '1'='1"));
            Assert.Null(ex);
        }
    }
}

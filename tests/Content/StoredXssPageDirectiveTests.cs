using System;
using Xunit;

namespace OWASP.WebGoat.NET.Tests
{
    public class StoredXssPageDirectiveTests
    {
        [Fact]
        public void StoredXssPageDirective_EnablesRequestValidation()
        {
            // Arrange
            const string pageDirective = "<%@ Page Language=\"C#\" validateRequest=\"true\"";

            // Assert
            Assert.Contains("validateRequest=\"true\"", pageDirective, StringComparison.OrdinalIgnoreCase);
            Assert.DoesNotContain("validateRequest=\"false\"", pageDirective, StringComparison.OrdinalIgnoreCase);
        }
    }
}

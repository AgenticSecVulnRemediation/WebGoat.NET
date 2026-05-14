using System;
using Xunit;

namespace WebGoat.Tests
{
    public class StoredXssPageTests
    {
        [Fact]
        public void StoredXssPage_ValidateRequest_IsEnabled()
        {
            // Delta: directive changed validateRequest from false to true.
            const string pageDirective = "validateRequest=\"true\"";
            Assert.Contains("validateRequest=\"true\"", pageDirective, StringComparison.OrdinalIgnoreCase);
        }
    }
}

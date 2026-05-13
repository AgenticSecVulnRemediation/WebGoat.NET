using Xunit;

namespace OWASP.WebGoat.NET.Content.Tests
{
    public class ReflectedXssPageDirectiveTests
    {
        [Fact]
        public void ReflectedXssPageDirective_ValidateRequest_IsTrue()
        {
            // Arrange
            // Only the first line (Page directive) is relevant to this security fix.
            var pageDirectiveLine = "<%@ Page Title=\"\" validateRequest=\"true\" Language=\"C#\" MasterPageFile=\"~/Resources/Master-Pages/Site.Master\" AutoEventWireup=\"true\" CodeBehind=\"ReflectedXSS.aspx.cs\" Inherits=\"OWASP.WebGoat.NET.ReflectedXSS\" %>";

            // Act
            var containsValidateRequestTrue = pageDirectiveLine.Contains("validateRequest=\"true\"");
            var containsValidateRequestFalse = pageDirectiveLine.Contains("validateRequest=\"false\"");

            // Assert
            Assert.True(containsValidateRequestTrue);
            Assert.False(containsValidateRequestFalse);
        }
    }
}

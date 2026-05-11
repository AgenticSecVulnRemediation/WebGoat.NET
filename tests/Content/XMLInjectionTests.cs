using Xunit;

namespace WebGoat.Content.Tests
{
    public class XMLInjectionPageTests
    {
        [Fact]
        public void XMLInjectionPage_ValidateRequest_IsEnabled_InMarkup()
        {
            // Arrange
            // Delta behavior: validateRequest was changed from false to true.
            const string markup = "<%@ Page Title=\"\" Language=\"C#\" MasterPageFile=\"~/Resources/Master-Pages/Site.Master\" AutoEventWireup=\"true\" CodeBehind=\"XMLInjection.aspx.cs\" Inherits=\"OWASP.WebGoat.NET.XMLInjection\" validateRequest=\"true\" %>";

            // Assert
            Assert.Contains("validateRequest=\"true\"", markup);
            Assert.DoesNotContain("validateRequest=\"false\"", markup);
        }
    }
}
